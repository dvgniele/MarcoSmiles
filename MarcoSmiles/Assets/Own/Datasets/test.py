import pandas as pd
import numpy as np

dataset_array = pd.read_csv('marcosmiles_dataset.csv', sep=',', header=None)
x = dataset_array.copy()
y = dataset_array.copy()

xcols = [18]
ycols = [0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17]
x.drop(x.columns[xcols], axis=1, inplace=True)
y.drop(y.columns[ycols], axis=1, inplace=True)

print(y)

np.savetxt('test.out', y, delimiter=',')

a = input(' ')