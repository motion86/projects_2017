# -*- coding: utf-8 -*-
"""
Spyder Editor

This is a temporary script file.
"""

import pandas as pd
import matplotlib.pyplot as plt

def test_run():
    df = pd.read_csv("data/AMD.csv")
    #print(df['Adj Close'])
    df[['High','Low']].plot()
    plt.show()
    
if __name__ == "__main__":
    test_run()