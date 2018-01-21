'use strict';


var app = angular.module('app', ['angularRangeSlider']);

app.controller('AppController',['$scope', function($s) {
  $s.lowerValue = 10; 
  $s.upperValue = 500;
  $s.items = [{
      name  : 'First Item',
      value : 500
    },
    {
      name  : 'Second Item',
      value : 200
    },
    {
      name  : 'Third Item',
      value : 700
    }];

}]);
