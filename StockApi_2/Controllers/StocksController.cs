using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using StockApi_2.Models;

namespace StockApi_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : Controller
    {
        private static List<Stock> Stock_L = new List<Stock>();

        private static  SortedDictionary<String, Stock> Stock_SM = new  SortedDictionary<String, Stock>();

        [HttpGet]
        public ResultModel getStock(string id)
        {
            var result = new ResultModel();
            try
            {
                if (String.IsNullOrEmpty(id))
                {
                    if (Stock_SM.Count > 0)
                    {
                        Stock_SM.OrderBy(s => s.Key);
                        result.Data = Stock_SM.Values;
                    }
                    else
                    {
                        result.Message = "Data is empty.";
                    }
                }               
                else if (Stock_SM.ContainsKey(id))
                {
                    result.Data = Stock_SM[id];
                }
                else
                {
                    result.Message = "No matching results.";
                }
                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Message = e.Message;
            }

            return result;
        }

        [HttpPost]
        public ResultModel PostMult([FromBody] List<Stock> stocks)
        {
            var result = new ResultModel();
            try
            {
                List<object> temp_L = new List<object>();
                foreach (Stock s in stocks)
                {
                    temp_L.Add(Post(s).Data);

                }
                result.Data = temp_L;
                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Message = e.Message;
            }
            return result;
        }

        public ResultModel Post(Stock stock)
        {
            var result = new ResultModel();
            try
            {
                if (!Stock_SM.ContainsKey(stock.Id))
                {
                    Stock_SM[stock.Id] = stock;
                }
                else
                {
                    SortedDictionary<string,Dividend> Dividend_SM = new SortedDictionary<string, Dividend>();
                    foreach (Dividend d in Stock_SM[stock.Id].DividendData)
                    {
                        Dividend_SM.Add(d.Year,d);
                    }
                    

                    foreach (Dividend d in stock.DividendData)
                    {
                        if (Dividend_SM.ContainsKey(d.Year))
                        {
                            Stock_SM[stock.Id].DividendData.Remove(Dividend_SM[d.Year]);
                            Stock_SM[stock.Id].DividendData.Add(d);
                        }
                        else{
                            Stock_SM[stock.Id].DividendData.Add(d);
                        }   
                    }
                }
                Stock_SM[stock.Id].DividendData = Stock_SM[stock.Id].DividendData.OrderByDescending(d => d.Year).ToList();
                result.Data = Stock_SM[stock.Id];
                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Message = e.Message;
            }

            
            return result;
        }
    }
}
