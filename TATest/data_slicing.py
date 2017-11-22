# -*- coding: utf-8 -*-
"""
Created on Thu Nov 16 17:42:44 2017

@author: Myers
"""

import util as u

def test_run():
    #Define a date range
    dates = u.pd.date_range('2017-01-01', '2017-11-16')
    
    # Choose stock symbols to read
    symbols = ['AMD', 'MSFT', 'NRZ'] #SPY will be added in get_data()
    
    # Get stock data
    df = u.get_data(symbols, dates)
    
    # Slice by row range (dates) using DataFrame.ix[] selector
    #u.plot_selected(df, ['AMD', 'MSFT'], '2017-01-01', '2017-01-31', True)
    
    
    td = df.loc['2017-01-01':'2017-01-31', ['AMD', 'MSFT']]
    print(td) # Get the month of January
    #print(df[['AMD', 'MSFT']])
    
    # Normalize data divide all rows by the first row
    td = td/td.ix[0]
    
    # More explicit vesion of above td = td/td.ix[0, :]
    
    u.plot_data(td)
    
if __name__ == "__main__":
    test_run()

