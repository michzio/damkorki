export enum OffersType {
                   // bits positions: 7654 3210
        Random = 1,         // 2^0 => 0000 0001
        Recommanded = 2,    // 2^1 => 0000 0010
        Local = 4,          // 2^2 => 0000 0100
        Given = 8           // 2^3 => 0000 1000

        // then you can bitwise OR different values like this
        // .Random | .Local  => 1 + 4 = 5  
        //  i.e. 
        // 0000 0001
        // 0000 0100 + 
        // ---------
        // 0000 0101 <= and you have this information about 2 types exactly in one number = 5! 
}