using Lab1.CBR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace Lab1
{
    class CBRService : INotifyPropertyChanged
    {
        private CBR.DailyInfo _cbrClient;
        private decimal lastQueriedRate;
        public event PropertyChangedEventHandler PropertyChanged;

        public CBR.DailyInfo Cbr
        {
            get
            {
                if (_cbrClient == null)
                {
                    _cbrClient = new DailyInfo();
                }
                _cbrClient.GetCursOnDateCompleted += new GetCursOnDateCompletedEventHandler(_cbrClient_GetCursOnDateCompleted);
                return _cbrClient;
            }
        }

        public decimal LastQueriedRate
        {
            set
            {
                if (lastQueriedRate == value) return;
                lastQueriedRate = value;
                NotifyPropertyChanged("LastQueriedRate");
            }
            get
            {
                return lastQueriedRate;
            }
        }

        private static decimal ExtractCurrencyRate(DataSet ds, string currencyCode)
        {
            if (ds == null)
                throw new ArgumentNullException("ds", "Параметр ds не может быть null.");

            if (string.IsNullOrEmpty(currencyCode))
                throw new ArgumentNullException("currencyCode", "Параметр currencyCode не может быть null.");

            DataTable dt = ds.Tables["ValuteCursOnDate"];

            DataRow[] rows = dt.Select(string.Format("VchCode=\'{0}\'", currencyCode));

            if (rows.Length > 0)
            {
                decimal result;
                if (decimal.TryParse(rows[0]["Vcurs"].ToString(), out result))
                    return result;
                throw new InvalidCastException("От службы ожидалось значение курса валют.");
            }
            throw new KeyNotFoundException("Для заданной валюты не найден курс.");
        }

        public decimal GetCurrencyRateOnDate(DateTime date, string currencyCode)
        {
            return ExtractCurrencyRate(Cbr.GetCursOnDate(date), currencyCode);
        }

        public void AsyncGetCurrencyRateOnDate(DateTime dateTime, string currencyCode)
        {
            Cbr.GetCursOnDateAsync(dateTime, currencyCode);
        }

        void _cbrClient_GetCursOnDateCompleted(object sender, GetCursOnDateCompletedEventArgs e)
        {
            LastQueriedRate = ExtractCurrencyRate(e.Result, (string)e.UserState);
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
