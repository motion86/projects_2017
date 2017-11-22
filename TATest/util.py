# -*- coding: utf-8 -*-
"""
Created on Thu Nov 16 17:18:56 2017

@author: Myers
"""

import os
import pandas as pd
import matplotlib.pyplot as plt

def symbol_to_path(symbol, base_dir="data"):
    #Return CSV file path given ticker symbol.
    return os.path.join(base_dir, "{}.csv".format(str(symbol)))

def get_data(symbols, dates, fillGaps=False):
    #Read stock data (adjusted close) for given symbols from CSV files.
    df = pd.DataFrame(index=dates)
    if 'SPY' not in symbols:  # add SPY for reference, if absent
        symbols.insert(0, 'SPY')

    for symbol in symbols:
        # TODO: Read and join data for each symbol
        dfSym = pd.read_csv(symbol_to_path(symbol), index_col="Date", parse_dates=True, usecols=['Date', 'Adj Close'], na_values=['nan'])
        dfSym = dfSym.rename(columns={'Adj Close' : symbol})
        if(symbol == 'SPY'):
            df = df.join(dfSym, how='inner')
        else:
            df = df.join(dfSym)
            
        if fillGaps:
            fill_missing_values(df)

    return df

def fill_missing_values(df_data):
    """Fill missing values in data frame, in place."""
    ##########################################################
    # TODO: Your code here (DO NOT modify anything else)
    df_data.fillna(method="ffill", inplace=True)  
    df_data.fillna(method="bfill", inplace=True)  
    ##########################################################

def plot_data(df, title="Stock Prices", ylabel="Price"):
    # Plot stock prices
    ax = df.plot(title=title)
    ax.set_xlabel("Date")
    ax.set_ylabel(ylabel)
    
    plt.show() #must be called to show plot
    
def plot_selected(df, columns, start_index, end_index, normalize=False):
    """Plot the desired columns over index values in the given range"""
    df = df.loc[start_index : end_index, columns]
    # Normalize data divide all rows by the first row
    if(normalize):
        df = df/df.ix[0]
    
    plot_data(df)
        