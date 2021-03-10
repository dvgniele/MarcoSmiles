import numpy as np
from numpy import genfromtxt
import matplotlib.pyplot as plt

from sklearn import svm, datasets
from sklearn.model_selection import train_test_split
from sklearn.metrics import plot_confusion_matrix

import pandas as pd
import seaborn as sn

array_data = genfromtxt('confusion_grid_data.csv', delimiter=',')


df = pd.DataFrame(array_data)
X = df[df.columns]
y = df[df.columns]

sn.set(rc={'figure.figsize': (11.7, 8.27)}) 
sn.heatmap(df,
           annot=True,
           annot_kws={"size": 16},
           cmap='Blues',
           fmt='g'
           )

plt.xlabel('Predicted Label')
plt.ylabel('True Label')
plt.show()
