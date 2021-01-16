import os
import numpy as np
from sklearn.metrics import precision_recall_fscore_support as score
from sklearn.preprocessing import RobustScaler, StandardScaler
from sklearn.preprocessing import LabelEncoder
from sklearn.model_selection import GridSearchCV
from numpy import mean
from sklearn.metrics import accuracy_score
from sklearn.model_selection import train_test_split
from sklearn.neural_network import MLPClassifier
import pickle
from sklearn.ensemble import ExtraTreesClassifier
import matplotlib.pyplot as plt

import pandas as pd
from _datetime import datetime



with open('learning_time.txt','w') as lt:     
    now = datetime.now()
    # dd/mm/YY H:M:S
    dt_string = now.strftime("%d/%m/%Y %H:%M:%S")
    lt.write(str("Data Inizio Learning: " + dt_string + "\n" ))


def feature_importance(X,y):
    '''mostra a video il grafico con le feature pi� importanti.
    Ogni feature � identificata con l'indice che occupa nell'array sample
    per salvare de-commentare una delle ultime righe
    '''

    # Build a forest and compute the impurity-based feature importances
    forest = ExtraTreesClassifier(n_estimators=250,
                                  random_state=0)

    forest.fit(X, y)
    importances = forest.feature_importances_
    std = np.std([tree.feature_importances_ for tree in forest.estimators_],
                 axis=0)
    indices = np.argsort(importances)[::-1]

    # Print the feature ranking
    print("Feature ranking:")

    for f in range(X.shape[1]):
        print("%d. feature %d (%f)" % (f + 1, indices[f], importances[indices[f]]))

    # Plot the impurity-based feature importances of the forest
    plt.figure()
    plt.title("Feature importances")
    plt.bar(range(X.shape[1]), importances[indices],
            color="r", yerr=std[indices], align="center")
    plt.xticks(range(X.shape[1]), indices)
    plt.xlim([-1, X.shape[1]])
    #plt.show()
    #salvare figura, cambiare path
    #plt.savefig('foo.png')
    plt.savefig('foo.pdf')
    #plt.savefig('foo.png', bbox_inches='tight') #salva senza mettere bordi bianchi troppo spessi

def grid(cl,classifier,param_grid,n_folds,t_s_D,tLab_downsampled):
    with open(cl+".out.txt","w") as f: # cambiare il percorso dove si salvano i vari esperimenti di validation con k-fold cross-validation

        #####   CAMBIARE NUMERO DI JOBS A SECONDA DELLA PROPRIA CPU ALTRIMENTI SKRT, ORA STA MESSO 20 SOLO PERCHE' VOGLIO FLEXARE 
        estimator = GridSearchCV(classifier, cv=n_folds, param_grid=param_grid, n_jobs=5, verbose=1,scoring='f1_weighted')     
        estimator.fit(t_s_D, tLab_downsampled)
        means = estimator.cv_results_['mean_test_score']
        stds = estimator.cv_results_['std_test_score']
        best=[]
        max=0
        for meana, std, params in zip(means, stds, estimator.cv_results_['params']):
            if meana>max:
                max=meana
                best=params
            print("%0.3f (+/-%0.03f) for %r" % (meana, std * 2, params))
            f.write("%0.3f (+/-%0.03f) for %r" % (meana, std * 2, params))
            f.write("\n")
        print()
    return best



#carico il dataset

#x = pickle.load(open("/path","rb")) #path da cambiare
#y = pickle.load(open("/path","rb")) #path da cambiare

'''
Il dataset deve avere questa forma. Preferibilmente un np.array
Ricordare che np.array non si pu� dichiarare vuoto e riempire mano mano,
ma deve essere dichiarato es. una riga di X elementi dove X � input np.zeros(18), cio� 18 elementi tutti 0
e successivamente accodare i sample/ gli elementi per riga con concatenate oppure append - trovare sintassi - 
ricordarsi di cancellare prima riga con tutti 0. 


X esempio (� una matrice) ogni riga � un elemento/sample
__________________________
| 3 5 7 9 12 9 1 0.5 0.1 0| elemento 1 del dataset
| 1 2 5 9 12 8 1 0.6 0.2 1| elemento 2 del dataset
| ...                     |
| 1 3 7 8 11 8 2 0.3 0.2 0| elemento n del dataset
___________________________


y esempio (� un array colonna) definisce la classe/label/nota di ciascun elemento/sample
___
|1| classe dell'elemento 1
|2| classe dell'elemento 2
|1| classe dell'elemento 3
|0| classe dell'elemento 4
|1| classe dell'elemento 5
|3| classe dell'elemento 6
|...|
|0| classe dell'elemento n
  
'''


dataset_array = pd.read_csv('marcosmiles_dataset.csv', sep=',', header=None)
x = dataset_array.copy()
y = dataset_array.copy()

xcols = [18]
ycols = [0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17]
x.drop(x.columns[xcols], axis=1, inplace=True)
y.drop(y.columns[ycols], axis=1, inplace=True)





#print(y)
'''
#se le label sono stringhe o non sono numerate da 0
label_encoder = LabelEncoder()
y = label_encoder.fit_transform(y)
y = y.reshape(len(y),) #(righe,colonne) (default=1)

print(y)
'''


#1 divido il dataset in modo stratificato in una parte per il training (80%) ed una parte per il testing (20%)
training_set_data,test_set_data,training_set_labels,test_set_labels = train_test_split(x,y,test_size=0.2,stratify=y)

#4 Scaling dei dati se non sono normalizzati. Vedere se � meglio Robust o Standard
scaler = StandardScaler()
train_scaled_D = scaler.fit_transform(training_set_data)
test_scaled_D = scaler.transform(test_set_data)


#feature importance
feature_importance(training_set_data,training_set_labels)


#5 validation con k-fold cross-validation
n_folds=10


#classificatore
classificatore=MLPClassifier()
#parametri da testare, sostituire numero hidden layer in base a grandezza input, sostituire max_iter in base a grandezza dataset
pg = {'activation' : ['tanh', 'relu'],'learning_rate' : ['invscaling','adaptive','constant'],
'solver' : ['adam','lbfgs','sgd'],
              'learning_rate_init':[0.01, 0.05, 0.1, 0.15, 0.2], 'hidden_layer_sizes':[3, 5, 10, 15, 17],
      'max_iter':[100, 400, 600]}
bestMLPparam=grid("marcosmiles_dataset.csv",classificatore,pg,n_folds,training_set_data,training_set_labels)
print("I migliori parametri per MLP sono:")
print(bestMLPparam)



classificatore= MLPClassifier(activation=bestMLPparam['activation'],learning_rate=bestMLPparam['learning_rate'],solver=bestMLPparam['solver'],
                             learning_rate_init=bestMLPparam['learning_rate_init'],max_iter=bestMLPparam['max_iter'],
                             hidden_layer_sizes=bestMLPparam['hidden_layer_sizes'])
classificatore.fit(training_set_data,training_set_labels)
y_pred=classificatore.predict(test_set_data)
precision,recall,fscore,support=score(test_set_labels,y_pred)
accuracy=accuracy_score(test_set_labels,y_pred)
print("MLP")
print("Accuracy")
print(accuracy)
print('precision: {}'.format(precision))
print('recall: {}'.format(recall))
print('fscore: {}'.format(fscore))
print("Weight e Bias")
print(classificatore.coefs_) #The ith element in the list represents the weight W matrix corresponding to layer i.
print(classificatore.intercepts_) #The ith element in the list represents the bias B vector corresponding to layer i + 1.

with open('precision_out.txt','w') as aw:       
    aw.write("Accuracy: ")
    aw.write(str(accuracy))
    aw.write("\n")
    aw.write("Precision: ")
    aw.write(str(precision))
    aw.write("\n")
    aw.write("Recall: ")
    aw.write(str(recall))
    aw.write("\n")
    aw.write("fscore: ")
    aw.write(str(fscore))
    aw.write("\n")
    
    



with open('weights_out.txt','w') as fw:       # new line \n identifica l'inizio di un nuovo layer
    for w_layer in classificatore.coefs_:
        fw.write(str(w_layer))
        fw.write("\n")
        
with open('bias_out.txt','w') as fb:       # new line \n identifica l'inizio di un nuovo layer, inizia dal layer i + 1
    for b_layer in classificatore.intercepts_:
        fb.write(str(b_layer))
        fb.write("\n")
        

with open('learning_time.txt','a+') as lt:     
    now = datetime.now()
    # dd/mm/YY H:M:S
    dt_string = now.strftime("%d/%m/%Y %H:%M:%S")
    lt.write(str("Data Fine Learning: " + dt_string + "\n"))
