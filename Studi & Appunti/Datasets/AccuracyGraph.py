import numpy as np
import pandas as pd
import matplotlib.pyplot as plt

file = open('precision_out.txt', 'r')
lines = file.read()
file.close()
accuracy_line = lines.split('\n')[0]
lines = lines.replace(accuracy_line, '')

accuracy = accuracy_line.split(':')[1].strip()


precision_block = lines.split('Recall')[0].split(':')[1].strip()
precision_lines = precision_block.replace('[', '')
precision_lines = precision_lines.replace(']', '')
precision_numbers = precision_lines.split(' ')

precision_array = []
for x in precision_numbers:
    if(x is not ''):
        x = (float)(x.replace('\n', ''))
        precision_array.append(x)


recall_block = lines.split('Recall')[1].split(':')[1].strip()
recall_block = recall_block.split('fscore')[0].strip()

recall_lines = recall_block.replace('[', '')
recall_lines = recall_lines.replace(']', '')
recall_numbers = recall_lines.split(' ')

recall_array = []
for x in recall_numbers:
    if(x is not ''):
        x = (float)(x.replace('\n', ''))
        recall_array.append(x)


fscore_block = lines.split('fscore')[1].split(':')[1].strip()
fscore_lines = fscore_block.replace('[', '')
fscore_lines = fscore_lines.replace(']', '')
fscore_numbers = fscore_lines.split(' ')

fscore_array = []
for x in fscore_numbers:
    if(x is not ''):
        x = (float)(x.replace('\n', ''))
        fscore_array.append(x)


txt = '\n\nAccuracy: ' + accuracy + '\n'
txt += 'Feature n.\tPrecision\tRecall\t\tfscore\n'
for i in range(len(precision_array)):
    precstr = str(precision_array[i])
    if len(precstr) <= 4:
        precstr += '\t'

    recallstr = str(recall_array[i])
    if len(recallstr) <= 4:
        recallstr += '\t'

    fscorestr = str(fscore_array[i])
    if len(fscorestr) <= 4:
        fscorestr += '\t'

    txt += '\t' + str(i) + '\t\t' + \
        precstr + '\t' + recallstr + '\t' + fscorestr + '\n'

print(txt)


a = input()
