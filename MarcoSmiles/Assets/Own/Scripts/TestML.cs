﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public static class TestML
{
    public static double[] ReteNeurale(double[] features)
    {
        var W1 = new double[][]
        {
            new double[]{
                0.1007911, 0.17900273, -0.06193091, 0.22252459, 0.16601526, -0.06660582,
                -0.1399435, -0.40749671, 0.01260347,  -0.10545862, -0.24585611, 0.13379333,
                -0.08301644, -0.09569832, -0.02766923
            },
            new double[]
            {
                0.15999694, 0.00271529, -0.15837409, -0.14102589, -0.3734263, -0.34960469,
                0.3230666, 0.18107898, 0.09646305, -0.27857117, 0.32301564, 0.30474104,
                0.22217171, -0.0947774, 0.15140401
            },
            new double[]
            {
                0.27942405, 0.13382463, -0.11304823, 0.23632906, 0.03179768, 0.14867222,
                -0.0735015, 0.2132005, 0.2597984, -0.29234424, 0.28038636, 0.50492264,
                -0.327143, 0.17205433, -0.0132988
            },
            new double[]
            {
                0.18598926, -0.03057061, -0.09723033, 0.10762237, 0.36972349, -0.23069245,
                -0.00115472, 0.10673261, 0.27820598, -0.16757461, -0.32091255, 0.24835379,
                -0.14012598, 0.29251954, -0.10392412
            },

            new double[]
            {
                -0.13774151, 0.09530078, -0.12364558, 0.24929884, 0.36615274, -0.5836104,
                0.20469956, -0.07671615, -0.19174271, 0.28055674, 0.30053675, -0.08752245,
                -0.21704149, -0.25581413, -0.03769555
            },

            new double[]
            {
                -0.33411998, -0.08548838, -0.13917797, 0.07867882, 0.246812, -0.5657792,
                -0.35327526, -0.33908516, -0.26462948, 0.12799844, -0.34755724, -0.13929008,
                0.20523486, 0.42562865, -0.11911697
            },

            new double[]
            {
                -0.44607619, 0.13923103, -0.36220043, -0.14152136, -0.13197692, 0.05594422,
                -0.04956238, -0.56742997, 0.16231576, 0.13682766, 0.05228764, 0.06018489,
                0.49411846, 0.16414441, -0.18480035
            },

            new double[]
            {
                -0.36116798, 0.26636435, -0.31425901, 0.15551427, 0.17566743, -0.49045602,
                -0.48212798, -0.4340317, -0.01192797, -0.09103974, 0.06582188, -0.24738114,
                0.26062645, -0.25655838, -0.20995101
            },

            new double[]
            {
                -0.12529577, 0.27607162, -0.35233575, -0.40966924, 0.40413831, -0.02946201,
                -0.45921937, -0.38770925, -0.21953209, -0.09000203, 0.33183682, 0.32910494,
                -0.21066674, -0.11880996, -0.09241438
            },

            new double[]
            {
                0.1577228, -0.13067931, 0.56977218, -0.07285146, -0.00130988, 0.12349916,
                0.09734657, 0.01792706, -0.36486, -0.41273489, 0.36070078, -0.04402264,
                0.27442882, -0.03470669, -0.16599929
            },

            new double[]
            {
                -0.39450784, 0.04707661, 0.03588396, -0.05919358, -0.47073881, 0.10932037,
                -0.05808955, -0.25246444, -0.07461776, -0.41611299, 0.07899162, 0.28788053,
                -0.21470507, 0.1942274, 0.00704991
            },

            new double[]
            {
                 0.32239774, -0.51553551, 0.08672088, -0.19670399, -0.20721171, -0.14998742,
                -0.0509207, 0.21595754, 0.29636355, -0.15055512, -0.12581056, -0.3800525,
                -0.27990246, -0.21300846, -0.11244059
            },

            new double[]
            {
                0.22726082, -0.39831209, 0.14304862, -0.00710359, 0.08198975, -0.25625924,
                0.31029824, -0.0838336, 0.14491164, -0.13685655, 0.02492582, -0.41138229,
                0.19678159, -0.30465459, -0.15041888
            },

            new double[]
            {
                -0.40094685, 0.05472953, 0.30404831, 0.02875321, 0.01531735, 0.16124148,
                0.03922736, -0.23843352, -0.31683341, 0.08744959, -0.39097604, -0.17449853,
                -0.04322055, 0.12381993, -0.04921295
            },

            new double[]
            {
                -0.32843019, -0.14748114, 0.07839891, -0.17221297, -0.45195059, 0.00554613,
                -0.57455554, -0.41724762, 0.31999218, 0.00257321, 0.02104117, 0.02995862,
                -0.02012971, -0.0450018, -0.06002417
            },

            new double[]
            {
                0.54043674, 0.07137639, 0.02033451, -0.33219297, -0.05409458, -0.00509477,
                -0.30334546, 0.11089016, 0.1295035, 0.0077941, 0.18839981, -0.08292618,
                0.25166827, -0.15305972, 0.04408515
            },

            new double[]
            {
                0.18858259, -0.30251431, 0.10286785, -0.28048604, -0.00231508, -0.02363068,
                -0.12792452, 0.21726126, -0.08651556, -0.0637831, -0.06187369, -0.04339686,
                0.34358131, 0.02472528, -0.05194613
            },

            new double[]
            {
                0.04487059, -0.00735255, -0.27371121, 0.09378713, 0.02484748, -0.5593687,
                0.15997938, 0.18803908, 0.11186996, 0.15082504, -0.0749515, -0.1247354,
                -0.26134022, -0.27949662, -0.18687531
            }



        };

        var B1 = new double[][]
        {
            new double[]
            {
                -0.42381459, 0.23535611, -0.74888681, 0.10036356, 0.16283649, 0.03082952,
                -0.01362044, -0.15004871, 0.1123109, -0.23832611, 0.08209701, -0.44509756,
                0.35717264, -0.63315696, 0.35160417
            }
        };

        var W2 = new double[][]
        {
            new double[]{
                -0.182809475, -0.182203933, 0.315318103, 0.319717142, 0.115788438, 0.156306339,
                -0.0720877887, -0.0457656772, -0.0458832812, -0.308210656, 0.00372011321, 0.0893403297
            },
            new double[]
            {
                -0.103543014, -0.0226896928, 0.0239528874, 0.393333618, 0.0425957784, 0.0942286988,
                0.25914325, -0.00752229542, -0.180773481, -0.0979662605, 0.443516241, 0.168352923
            },
            new double[]
            {
                0.0833677354, 0.148730218, -0.0256045861, 0.0533143101, 0.105484368, -0.148332231, 
                0.147129576, 0.0483280425, 0.186765793, -0.110626194, -0.12230173, -0.173105632
            },
            new double[]
            {
                0.271244437, -0.258193182, -0.189296623, -0.248288904, 0.231272547, -0.0672018758,
                0.33438937, -0.0274800608, 0.318194546, -0.0514894365, 0.351187007, 0.0306123245
            },

            new double[]
            {
                -0.52139474, -0.000641080518, 0.33107595, -0.493952161, 0.185714938, 0.397698684,
                0.218322247, 0.163103073, -0.0567602261, 0.165212393, -0.430783606, 0.0573948295
            },

            new double[]
            {   
                0.0943952221, 0.0760369456, 0.0207111746, 0.157196053, -0.362016729, 0.200996012,
                -0.0888814997, 0.0226081492, -0.104949524, -0.10825461, 0.302842997, 0.28662486
            },

            new double[]
            {
                -0.0821469489, -0.28148441, -0.0612207727, 0.0721086926, 0.178240022, 0.637876719, 
                0.0473511979, 0.0363042981, 0.17287111, 0.216859132, -0.142862045, -0.0223539779
            },

            new double[]
            {
                0.331299434, -0.254941059, 0.281553175, 0.077075245, -0.410402779, -0.0241129884,
                -0.139140382, 0.166081805, -0.171288946, 0.327354495, -0.0319086199, -0.159574632
            },

            new double[]
            {
                0.064944667, 0.307642793, -0.19705422, 0.149355457, -0.332575553, -0.212508196,
                -0.388421351, -0.540482675, -0.541987089, -0.0666808201, 0.0481908069, 0.2749591
            },


            new double[]
            {
                -0.181015011, -0.0000000001, 0.0000000001, 0.0000000001, -0.0000000027, 0.441613222, 
                -0.0656601752, 0.0000599372, 0.169782503, -0.0000373813, 0.0000000001, 0.0000000001
            },

            new double[]
            {
                 -0.319731305, 0.00630603349, -0.0680454472, -0.528856498, 0.0705656566, -0.75351231,
                 0.184206833, -0.00692613637, 0.491760445, -0.456439004, 0.392934283, -0.166914986
            },

            new double[]
            {
                0.172220832, -0.339235882, -0.0942209743, 0.0239412154, -0.212884127, 0.24203269,
                -0.363770514, 0.514050565, 0.271990482, 0.426809065, 0.259562689, -0.0148263944
            },

            new double[]
            {
               -0.31941457, 0.170013282, -0.0413473389, -0.0126883268, 0.369430374, -0.225528703,
                0.651235199, 0.162152519, 0.288784612, -0.277919953, -0.948669672, 0.265851061
            },

            new double[]
            {
                0.374576486, -0.271191355, -0.088027043, 0.219877981, 0.29757555, 0.0670372903,
                -0.494612657, -0.0674608559, -0.287996806, 0.239318431, -0.147537045, -0.179018618
            },

            new double[]
            {
                -0.163850538, -0.000000730, 0.00293516496, 0.00812371848, -0.106608725, 0.00497186352,
                0.0000000001, 0.0657407746, -0.0923276946, 0.216043581, -0.0368627355, -0.277114599
            },
        };

        var B2 = new double[][]
        {
            new double[]
            {
                -0.0096941, 0.61652314, 0.22538885, 0.0519365, 0.39038265, -0.09174195,
                -0.28781865, 0.01863683, 0.2438597, -0.37556753, 0.51495179, -0.18689297
            }
        };

        var output_hidden1 = new double[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
        var output_hidden2 = new double[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };

        //double[] scaledFeatures = ScaleValues(features);


        double x, w, r;
        var flag = false;
        for(int i = 0; i < output_hidden1.Length; i++)
        {
            output_hidden1[i] += B1[0][i];
            for(int j = 0; j < features.Length; j++)
            {
                x = features[j];
                w = W1[j][i];
                r = x * w;

                output_hidden1[i] += r;
            }

            if (output_hidden1[i] <= 0)
                output_hidden1[i] = 0;
        }

        for(int i = 0; i<output_hidden2.Length; i++)
        {
            output_hidden2[i] += B2[0][i];
            for(int j = 0; j < output_hidden1.Length; j++)
            {
                x = output_hidden1[j];
                w = W2[j][i];
                r = x * w;

                output_hidden2[i] += r;
            }
        }

        double sum = 0;
        foreach (var item in output_hidden2)
        {
            sum += Mathf.Exp((float)item);
        }

        var toRet = new double[output_hidden2.Length];
        for(int i =0; i< output_hidden2.Length; i++)
        {
            toRet[i] = Mathf.Exp((float)output_hidden2[i]) /sum; 
        }

        return toRet;
    }


    private static double[] ScaleValues(double[] unscaledFeatures)
    {
        var scaledFeatures = new double[unscaledFeatures.Length];
        var minValues = new double[unscaledFeatures.Length];
        var maxValues = new double[unscaledFeatures.Length];

        string[] readText = File.ReadAllLines("Assets/Own/Datasets/min&max_values_dataset_out.txt");        //primo elemento contiene riga contenente valori min
                                                                                                            //secondo elemento contiene riga contenente valori max
        string[] min = readText[0].Split(' ');
        string[] max = readText[1].Split(' ');

        for (int i=0 ; i<unscaledFeatures.Length; i++)
        {
            minValues[i] = Double.Parse(min[i], CultureInfo.InvariantCulture);
            maxValues[i] = Double.Parse(max[i], CultureInfo.InvariantCulture);
        }

        for (int i = 0; i < minValues.Length; i++)
        {
            Debug.Log("index " + i +" (valori min): " + minValues[i]);
        }
        for (int i = 0; i < maxValues.Length; i++)
        {
            Debug.Log("index " + i + " (valori max): " + maxValues[i]);
        }


        for (int i = 0; i<unscaledFeatures.Length; i++)
        {
            scaledFeatures[i] = (unscaledFeatures[i] - minValues[i]) / (maxValues[i] - minValues[i]);
        }
    /*
       foreach (double e in scaledFeatures){
            Debug.Log("features scalate  : " + e);
        }
 */
        return scaledFeatures;

    }




}
