# -*- coding: utf-8 -*-
"""
Created on Mon Nov 20 13:43:22 2017

@author: Myers
"""

import util as u
import pandas as pd

def test_run():
    #Define a date range
    dates = u.pd.date_range('2017-01-01', '2017-11-16')

    # Choose stock symbols to read
    symbols = ['SPY'] #SPY will be added in get_data()
    
    # Get stock data
    df = u.get_data(symbols, dates)
    
    # Plot SPY data, reatain plt axis object
    ax = df['SPY'].plot(title='SPY rolling mean', label='SPY')
    
    # Compute Bollinger Bands
    # 1. Compute rolling mean
    rm_SPY = get_rolling_mean(df['SPY'], window=20)

    # 2. Compute rolling standard deviation
    rstd_SPY = get_rolling_std(df['SPY'], window=20)

    # 3. Compute upper and lower bands
    upper_band, lower_band = get_bollinger_bands(rm_SPY, rstd_SPY)
    
    # Plot raw SPY values, rolling mean and Bollinger Bands
    ax = df['SPY'].plot(title="Bollinger Bands", label='SPY')
    rm_SPY.plot(label='Rolling mean', ax=ax)
    upper_band.plot(label='upper band', ax=ax)
    lower_band.plot(label='lower band', ax=ax)

    # Add axis labels and legend
    ax.set_xlabel("Date")
    ax.set_ylabel("Price")
    ax.legend(loc='upper left')
    u.plt.show()
    
    #Compute global statistics for each stock
    print('Stock Mean:')
    print(df.mean())
    print('Stock Median:')
    print(df.median())
    print('Stock Std:')
    print(df.std())
    
    symbols = ['SPY','AMD']
    df = u.get_data(symbols, dates)
    u.plot_data(df)

    # Compute daily returns
    daily_returns = compute_daily_returns(df)
    u.plot_data(daily_returns, title="Daily returns", ylabel="Daily returns")
    
    # Cumulative Returns
    df = u.get_data(symbols, dates)
    daily_returns = cumulative_returns(df)
    u.plot_data(daily_returns, title="Cumulative returns", ylabel="Cumulative returns")
    

def compute_daily_returns(df):
    """Compute and return the daily return values."""
    # TODO: Your code here
    # Note: Returned DataFrame must have the same number of rows
    df.values[1:, :] = df.values[1:, :] / df.values[:-1, :]
    df.values[0, :] = 1
    df = df * 100 - 100
    return df

def cumulative_returns(df):
    df.values[1:, :] = df.values[1:, :] / df.values[0, :] - 1
    df = df * 100 
    df.values[0, :] = 0
    return df
    
def get_rolling_mean(values, window):
    """Return rolling mean of given values, using specified window size."""
    return values.rolling(window=window).mean()


def get_rolling_std(values, window):
    """Return rolling standard deviation of given values, using specified window size."""
    # TODO: Compute and return rolling standard deviation
    return values.rolling(window=window).std()


def get_bollinger_bands(rm, rstd):
    """Return upper and lower Bollinger Bands."""
    # TODO: Compute upper_band and lower_band
    upper_band = rm + 2 * rstd
    lower_band = rm - 2 * rstd
    return upper_band, lower_band
    
if __name__ == '__main__':
    test_run()
    