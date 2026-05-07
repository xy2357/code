import pandas as pd
import numpy as np

pd.set_option('display.max_columns', None)
# df = pd.read_csv('nba.csv')
# print(df.to_string(index=False))

# df = pd.read_excel('ImpactConfig.xlsm', sheet_name='Buff', skiprows=range(1,6), index_col=0, nrows=110, engine = 'openpyxl')
df = pd.read_excel(
    'ImpactConfig.xlsm',
    sheet_name='Buff',
    skiprows=range(1, 6),
    index_col=0,
    nrows=110,
    engine='openpyxl'
)

print(df)