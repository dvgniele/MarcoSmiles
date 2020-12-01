function neural_network_G(feature_array) {
    // Xi soni i valori delle nostre features
    X = feature_array


    W1 = [[0.1007911, 0.17900273, -0.06193091, 0.22252459, 0.16601526, -0.06660582
        , -0.1399435, -0.40749671, 0.01260347, -0.10545862, -0.24585611, 0.13379333
        , -0.08301644, -0.09569832, -0.02766923],

        , [, 0.15999694, 0.00271529, -0.15837409, -0.14102589, -0.3734263, -0.34960469
        , 0.3230666, 0.18107898, 0.09646305, -0.27857117, 0.32301564, 0.30474104
        , 0.22217171, -0.0947774, 0.15140401],

        , [, 0.27942405, 0.13382463, -0.11304823, 0.23632906, 0.03179768, 0.14867222
        , -0.0735015, 0.2132005, 0.2597984, -0.29234424, 0.28038636, 0.50492264
        , -0.327143, 0.17205433, -0.0132988],

        , [, 0.18598926, -0.03057061, -0.09723033, 0.10762237, 0.36972349, -0.23069245
        , -0.00115472, 0.10673261, 0.27820598, -0.16757461, -0.32091255, 0.24835379
        , -0.14012598, 0.29251954, -0.10392412],

        , [-0.13774151, 0.09530078, -0.12364558, 0.24929884, 0.36615274, -0.5836104
        , 0.20469956, -0.07671615, -0.19174271, 0.28055674, 0.30053675, -0.08752245
        , -0.21704149, -0.25581413, -0.03769555],

        , [-0.33411998, -0.08548838, -0.13917797, 0.07867882, 0.246812, -0.5657792
        , -0.35327526, -0.33908516, -0.26462948, 0.12799844, -0.34755724, -0.13929008
        , 0.20523486, 0.42562865, -0.11911697],

        , [-0.44607619, 0.13923103, -0.36220043, -0.14152136, -0.13197692, 0.05594422
        , -0.04956238, -0.56742997, 0.16231576, 0.13682766, 0.05228764, 0.06018489
        , 0.49411846, 0.16414441, -0.18480035],

        , [-0.36116798, 0.26636435, -0.31425901, 0.15551427, 0.17566743, -0.49045602
        , -0.48212798, -0.4340317, -0.01192797, -0.09103974, 0.06582188, -0.24738114
        , 0.26062645, -0.25655838, -0.20995101],

        , [-0.12529577, 0.27607162, -0.35233575, -0.40966924, 0.40413831, -0.02946201
        , -0.45921937, -0.38770925, -0.21953209, -0.09000203, 0.33183682, 0.32910494
        , -0.21066674, -0.11880996, -0.09241438],

        , [, 0.1577228, -0.13067931, 0.56977218, -0.07285146, -0.00130988, 0.12349916
        , 0.09734657, 0.01792706, -0.36486, -0.41273489, 0.36070078, -0.04402264
        , 0.27442882, -0.03470669, -0.16599929],

        , [-0.39450784, 0.04707661, 0.03588396, -0.05919358, -0.47073881, 0.10932037
        , -0.05808955, -0.25246444, -0.07461776, -0.41611299, 0.07899162, 0.28788053
        , -0.21470507, 0.1942274, 0.00704991],

        , [, 0.32239774, -0.51553551, 0.08672088, -0.19670399, -0.20721171, -0.14998742
        , -0.0509207, 0.21595754, 0.29636355, -0.15055512, -0.12581056, -0.3800525
        , -0.27990246, -0.21300846, -0.11244059],

        , [, 0.22726082, -0.39831209, 0.14304862, -0.00710359, 0.08198975, -0.25625924
        , 0.31029824, -0.0838336, 0.14491164, -0.13685655, 0.02492582, -0.41138229
        , 0.19678159, -0.30465459, -0.15041888],

        , [-0.40094685, 0.05472953, 0.30404831, 0.02875321, 0.01531735, 0.16124148
        , 0.03922736, -0.23843352, -0.31683341, 0.08744959, -0.39097604, -0.17449853
        , -0.04322055, 0.12381993, -0.04921295],

        , [-0.32843019, -0.14748114, 0.07839891, -0.17221297, -0.45195059, 0.00554613
        , -0.57455554, -0.41724762, 0.31999218, 0.00257321, 0.02104117, 0.02995862
        , -0.02012971, -0.0450018, -0.06002417],

        , [, 0.54043674, 0.07137639, 0.02033451, -0.33219297, -0.05409458, -0.00509477
        , -0.30334546, 0.11089016, 0.1295035, 0.0077941, 0.18839981, -0.08292618
        , 0.25166827, -0.15305972, 0.04408515],

        , [, 0.18858259, -0.30251431, 0.10286785, -0.28048604, -0.00231508, -0.02363068
        , -0.12792452, 0.21726126, -0.08651556, -0.0637831, -0.06187369, -0.04339686
        , 0.34358131, 0.02472528, -0.05194613],
        
        , [, 0.04487059, -0.00735255, -0.27371121, 0.09378713, 0.02484748, -0.5593687
        , 0.15997938, 0.18803908, 0.11186996, 0.15082504, -0.0749515, -0.1247354
        , -0.26134022, -0.27949662, -0.18687531]];

    B1 = [[-0.42381459, 0.23535611, - 0.74888681, 0.10036356, 0.16283649, 0.03082952,
    - 0.01362044, - 0.15004871, 0.1123109, - 0.23832611, 0.08209701, - 0.44509756,
        0.35717264, - 0.63315696, 0.35160417]];

    W2 = [[-1.82809475e-01, -1.82203933e-01, 3.15318103e-01, 3.19717142e-01
        , 1.15788438e-01, 1.56306339e-01, -7.20877887e-02, -4.57656772e-02
        , -4.58832812e-02, -3.08210656e-01, 3.72011321e-03, 8.93403297e-02],

        , [-1.03543014e-01, -2.26896928e-02, 2.39528874e-02, 3.93333618e-01
        , 4.25957784e-02, 9.42286988e-02, 2.59143250e-01, -7.52229542e-03
        , -1.80773481e-01, -9.79662605e-02, 4.43516241e-01, 1.68352923e-01],

        , [, 8.33677354e-02, 1.48730218e-01, -2.56045861e-02, 5.33143101e-02
        , 1.05484368e-01, -1.48332231e-01, 1.47129576e-01, 4.83280425e-02
        , 1.86765793e-01, -1.10626194e-01, -1.22301730e-01, -1.73105632e-01],

        , [, 2.71244437e-01, -2.58193182e-01, -1.89296623e-01, -2.48288904e-01
        , 2.31272547e-01, -6.72018758e-02, 3.34389370e-01, -2.74800608e-02
        , 3.18194546e-01, -5.14894365e-02, 3.51187007e-01, 3.06123245e-02],

        , [-5.21394740e-01, -6.41080518e-04, 3.31075950e-01, -4.93952161e-01
        , 1.85714938e-01, 3.97698684e-01, 2.18322247e-01, 1.63103073e-01
        , -5.67602261e-02, 1.65212393e-01, -4.30783606e-01, 5.73948295e-02],

        , [, 9.43952221e-02, 7.60369456e-02, 2.07111746e-02, 1.57196053e-01
        , -3.62016729e-01, 2.00996012e-01, -8.88814997e-02, 2.26081492e-02
        , -1.04949524e-01, -1.08254610e-01, 3.02842997e-01, 2.86624860e-01],

        , [-8.21469489e-02, -2.81484410e-01, -6.12207727e-02, 7.21086926e-02
        , 1.78240022e-01, 6.37876719e-01, 4.73511979e-02, 3.63042981e-02
        , 1.72871110e-01, 2.16859132e-01, -1.42862045e-01, -2.23539779e-02],

        , [, 3.31299434e-01, -2.54941059e-01, 2.81553175e-01, 7.70752450e-02
        , -4.10402779e-01, -2.41129884e-02, -1.39140382e-01, 1.66081805e-01
        , -1.71288946e-01, 3.27354495e-01, -3.19086199e-02, -1.59574632e-01],

        , [, 6.49446670e-02, 3.07642793e-01, -1.97054220e-01, 1.49355457e-01
        , -3.32575553e-01, -2.12508196e-01, -3.88421351e-01, -5.40482675e-01
        , -5.41987089e-01, -6.66808201e-02, 4.81908069e-02, 2.74959100e-01],

        , [-1.81015011e-01, -4.33400729e-24, 7.84702283e-24, 8.09840396e-24
        , -2.73709228e-09, 4.41613222e-01, -6.56601752e-02, 5.99372465e-05
        , 1.69782503e-01, -3.73813606e-05, 1.15493723e-24, 1.12393630e-24],

        , [-3.19731305e-01, 6.30603349e-03, -6.80454472e-02, -5.28856498e-01
        , 7.05656566e-02, -7.53512310e-01, 1.84206833e-01, -6.92613637e-03
        , 4.91760445e-01, -4.56439004e-01, 3.92934283e-01, -1.66914986e-01],

        , [, 1.72220832e-01, -3.39235882e-01, -9.42209743e-02, 2.39412154e-02
        , -2.12884127e-01, 2.42032690e-01, -3.63770514e-01, 5.14050565e-01
        , 2.71990482e-01, 4.26809065e-01, 2.59562689e-01, -1.48263944e-02],

        , [-3.19414570e-01, 1.70013282e-01, -4.13473389e-02, -1.26883268e-02
        , 3.69430374e-01, -2.25528703e-01, 6.51235199e-01, 1.62152519e-01
        , 2.88784612e-01, -2.77919953e-01, -9.48669672e-01, 2.65851061e-01],

        , [, 3.74576486e-01, -2.71191355e-01, -8.80270430e-02, 2.19877981e-01
        , 2.97575550e-01, 6.70372903e-02, -4.94612657e-01, -6.74608559e-02
        , -2.87996806e-01, 2.39318431e-01, -1.47537045e-01, -1.79018618e-01],
        
        , [-1.63850538e-01, -7.30165750e-07, 2.93516496e-03, 8.12371848e-03
        , -1.06608725e-01, 4.97186352e-03, 2.74161573e-24, 6.57407746e-02
        , -9.23276946e-02, 2.16043581e-01, -3.68627355e-02, -2.77114599e-01]];



    B2 = [
        [-0.0096941, 0.61652314, 0.22538885, 0.0519365, 0.39038265, - 0.09174195,
        - 0.28781865, 0.01863683, 0.2438597, - 0.37556753, 0.51495179, - 0.18689297]
    ];


    // output_hidden1 ha lo stesso numero di elementi di B1
    var output_hidden1 = [0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0] 
    // stesso per hudden2 con B2
    var output_hidden2 = [0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0]
    var x, w, r
    var flag = false

    for (var i = 0; i < output_hidden1.length; i++) {

        output_hidden1[i] += B1[i][0]
        for (var j = 0; j < X.length; j++) {
            x = X[j]
            w = W1[j][i]
            r = x * w

            output_hidden1[i] += r
        }

        //activation function, qui tanh


        //  relu
        if (output_hidden1[i] <= 0) {
            output_hidden1[i] = 0;
        }

    }

    for (var i = 0; i < output_hidden2.length; i++) {

        output_hidden2[i] += B2[i][0]
        for (var j = 0; j < output_hidden1.length; j++) {
            x = output_hidden1[j]
            w = W2[j][i]
            r = x * w

            output_hidden2[i] += r
        }

    }

    /*
    
    def softmax(x):
    """Compute softmax values for each sets of scores in x."""
    return np.exp(x) / np.sum(np.exp(x), axis=0)

    */

    double sum = 0;
    foreach(var item in output_hidden2)
    {
        sum += Math.exp(item);
    }

    return Math.exp(output_hidden2) / sum;
}