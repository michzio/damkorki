/*
 * Webpack (JavaScriptServices) with a few changes & updates
 * - This is to keep us inline with JSServices, and help those using that template to add things from this one
 *
 * Things updated or changed:
 * module -> rules []
 *    .ts$ test : Added 'angular2-router-loader' for lazy-loading in development
 *    added ...sharedModuleRules (for scss & font-awesome loaders)
 */

const path = require('path');
const webpack = require('webpack');
const merge = require('webpack-merge');
//const AotPlugin = require('@ngtools/webpack').AotPlugin;
const AngularCompilerPlugin = require('@ngtools/webpack').AngularCompilerPlugin;
const CheckerPlugin = require('awesome-typescript-loader').CheckerPlugin;
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

const { sharedModuleRules } = require('./webpack.additions');

module.exports = (env) => {
    // Configuration in common to both client-side and server-side bundles
    const isDevBuild = !(env && env.prod);
    
    if(isDevBuild) { 
        console.log(`Running build for Development.`);
    }

    if(env && env.prod) { 
        console.log(`Running build for Production.`);
    }

    const sharedConfig = {
        stats: { modules: false },
        context: __dirname,
        resolve: { extensions: [ '.js', '.ts' ] },
        output: {
            filename: '[name].js',
            publicPath: 'dist/' // Webpack dev middleware, if enabled, handles requests for this URL prefix
        },
        module: {
            rules: [
                { test: /\.ts$/, use: isDevBuild ? ['awesome-typescript-loader?silent=true', 'angular2-template-loader', 'angular2-router-loader'] : '@ngtools/webpack' },
                { test: /\.html$/, use: 'html-loader?minimize=false' },
                { test: /\.css$/, use: [ 'to-string-loader', isDevBuild ? 'css-loader' : 'css-loader?minimize' ] },
                { test: /\.(png|jpg|jpeg|gif)$/, use: 'url-loader?limit=100000' },
                ...sharedModuleRules
            ]
        },
        plugins: [new CheckerPlugin()]
    };

    // Configuration for client-side bundle suitable for running in browsers
    const clientBundleOutputDir = './wwwroot/dist';
    const clientBundleConfig = merge(sharedConfig, {
        entry: { 'main-client': './Client/main.browser.ts' },
        output: { path: path.join(__dirname, clientBundleOutputDir) },
        plugins: [
            new webpack.DllReferencePlugin({
                context: __dirname,
                manifest: require('./wwwroot/dist/vendor-manifest.json')
            })
        ].concat(isDevBuild ? [
            // Plugins that apply in development builds only
            new webpack.SourceMapDevToolPlugin({
                filename: '[file].map', // Remove this line if you prefer inline source maps
                moduleFilenameTemplate: path.relative(clientBundleOutputDir, '[resourcePath]') // Point sourcemap entries to the original file locations on disk
            })
        ] : [
            // new BundleAnalyzerPlugin(),
            // Plugins that apply in production builds only
            new webpack.optimize.UglifyJsPlugin(),
            //new AotPlugin({
            new AngularCompilerPlugin({
                tsConfigPath: './tsconfig.json',
                entryModule: path.join(__dirname, 'Client/app/browser-app.module#BrowserAppModule'),
                exclude: ['./**/*.server.ts']
            })
        ]), 
        node: {
            fs: "empty"
        }
    });

    // Configuration for server-side (prerendering) bundle suitable for running in Node
    const serverBundleConfig = merge(sharedConfig, {
        // resolve: { mainFields: ['main'] },
        entry: { 'main-server': './Client/main.server.ts' },
        plugins: [
            new webpack.DllReferencePlugin({
                context: __dirname,
                manifest: require('./Client/dist/vendor-manifest.json'),
                sourceType: 'commonjs2',
                name: './vendor'
            })
        ].concat(isDevBuild ? [] : [
            new webpack.optimize.UglifyJsPlugin({
              compress: false,
              mangle: false
            }),
            // Plugins that apply in production builds only
            //new AotPlugin({
            new AngularCompilerPlugin({
                tsConfigPath: './tsconfig.json',
                entryModule: path.join(__dirname, 'Client/app/server-app.module#ServerAppModule'),
                exclude: ['./**/*.browser.ts']
            })
        ]),
        output: {
            libraryTarget: 'commonjs',
            path: path.join(__dirname, './Client/dist')
        },
        target: 'node',
        devtool: isDevBuild ? 'inline-source-map': false
    });

    return [clientBundleConfig, serverBundleConfig];
};