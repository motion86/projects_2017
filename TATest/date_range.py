# -*- coding: utf-8 -*-
"""
Created on Thu Nov 16 16:49:11 2017

@author: Myers
"""

import pandas as pd

def test_run():
    #Define date range
    start_date='2017-10-16'
    end_date='2017-11-16'
    dates = pd.date_range(start_date, end_date)
    
    #Create an empty dataframe
    df1 = pd.DataFrame(index=dates)
    
    #read SPY data into temp dataframe
    dfSPY = pd.read_csv("data/SPY.csv", index_col="Date", parse_dates=True, usecols=['Date', 'Adj Close'], na_values=['nan'])
    
    #Rename 'Adj Close' column to 'SPY' to prevent clash
    dfSPY = dfSPY.rename(columns={'Adj Close' : 'SPY'})
    #print(dfSPY)
    
    #Join the two dataframes using DataFrame.join()
    df1 = df1.join(dfSPY, how='inner')
    
    #read in more stocks
    symbols = ['AMD', 'MSFT', 'NRZ']
    
    for symbol in symbols:
        df_temp = pd.read_csv("data/{}.csv".format(symbol), index_col="Date", 
                              parse_dates=True, usecols=['Date', 'Adj Close'], 
                              na_values=['nan'])
        df_temp = df_temp.rename(columns={'Adj Close' : symbol})
        df1 = df1.join(df_temp)
    
    print(df1)df
    
if __name__ == "__main__":
    test_run()