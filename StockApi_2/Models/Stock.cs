using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StockApi_2.Models
{
    public class Stock
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public  List<Dividend> DividendData { get; set; }


    }

    public class Dividend
    {
        [Key]
        public string Year { get; set; }
        public double CashDividend { get; set; }
        public double StockDividend { get; set; }
        public double TotalDividend { get; set; }

    }
}
