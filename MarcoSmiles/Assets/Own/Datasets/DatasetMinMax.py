#carico il dataset
import numpy as np
import pandas as pd

dataset_array = pd.read_csv('marcosmiles_dataset.csv', sep=',', header=None)
ycols = [0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17]

min_values = []
max_values = []

for col in ycols:
    min_values.append(min(dataset_array[col]))
    max_values.append(max(dataset_array[col]))
    
with open('min&max_values_dataset_out.txt','w') as f:       
    for item_min in min_values:
        f.write(str(item_min) + " ")
    f.write("\n")
    for item_max in max_values:
        f.write(str(item_max) + " ")