# -*- coding: utf-8 -*-
"""
Created on Mon Nov 20 11:13:57 2017

@author: Myers
"""
import numpy as np
import time

def test_run():
    #List 50 1D array
    print(np.array([2,3,4]))
    # 5d Vector
    print(np.empty(5))
    # 2 rows 3 cols
    print(np.empty((2,3)))
    # 2 levels, 2 rows 3 cols each
    print(np.empty((2,3,2)))
    
    #array of ones
    print(np.ones((5,4), dtype=np.int))
    
    # random 5 Rows 4 Cols
    print(np.random.random((5,4)))
    
    # random 5 Rows 4 Cols
    print(np.random.rand(5,4))
    
    #random integers
    print(np.random.randint(2,40, size=(2,3)))
    
    # Standard Normal (mean = 0, s.d. = 1)
    print( np.random.normal(size=(2,3)) )
    
    # Normal (mean = 50, s.d. = 10)
    print( np.random.normal(50, 10, size=(2,3)) )
    
    # use time for timing stuff
    t1 = time.time()
    a = np.random.random((50,40)) # 5x 4 array of random numbers
    print(a.shape)
    #Number of rows
    print(a.shape[0])
    #Number of Cols
    print(a.shape[1])
    #number of elements in array
    print(a.size)
    # Sum all numbers in the array
    print(a.sum())
    # Iterate over rows, to compute sum of each column
    print("Sum of each column:\t", a.sum(axis=0))
    # Iterate over cols, to compute sum of each row
    print("Sum of each row:\t", a.sum(axis=1))
    
    # Statistics: min,max,mean (across row, cols, and overall)
    print("Minimum of each col:\t", a.min(axis=0))
    print("Max of each row:\t", a.max(axis=1))
    print('Mean of all elements', a.mean()) # can also be calculated for each row or col using axis
    print('Index of max value in arr', a.argmax()) # takes axis as well
    t2 = time.time()
    print('it took:\t', t2-t1)
    
    b = np.random.rand(5)
    #accessing using list of indecies.. fils the passed array with the elements at given indecies
    indecies = np.array([1,1,2,3])
    
    print('Arr B = ', b)
    
    mb = b.mean()
    #give me all elements less then the mean
    print('All elem of B less then the mean:', b[b< mb])
    b[b< mb] = mb
    print('All elem of B less then the mean assigned to the mean:', b )
    
    
if(__name__ == "__main__"):
    test_run()

