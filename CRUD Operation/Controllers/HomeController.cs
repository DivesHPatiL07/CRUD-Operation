using CRUD_Operation.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Net.Http.Headers;

namespace CRUD_Operation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public ProductListModel GetProductList(string search, string sort, string order, int offset, int limit)
        {
            ProductListModel rtnValues = new ProductListModel();
            string TableName = "VW_PRODUCT";
            string WhereCondition = "";
            string ColumnsToSelect = "ID,PRODUCT,DESCRIPTION,CREATEDDATE";


            if (!string.IsNullOrEmpty(search))
            {
                string where = "";
                whereClause("PRODUCT", search, ref where);
                whereClause("DESCRIPTION", search, ref where);
                whereClause("CREATEDDATE", search, ref where);
                WhereCondition = where;
            }
            else
            {
                WhereCondition = "";
            }
            int PageSize = limit == 0 ? 10000 : limit;
            int PageNumber = offset == 0 ? 1 : (offset / limit) + 1;
            string OrderByColumn = sort == null ? "ID" : sort;
            string SortOrder = order == null ? "ASC" : order;

            try
            {
                var con = DbHandlerBase.GetConnection();
                con.Open();

                var P = new DynamicParameters();
                P.Add("@TableName", TableName);
                P.Add("@ColumnsToSelect", ColumnsToSelect);
                P.Add("@WhereCondition", WhereCondition);
                P.Add("@PageSize", PageSize);
                P.Add("@PageNumber", PageNumber);
                P.Add("@OrderByColumn", OrderByColumn);
                P.Add("@SortOrder", SortOrder);
                P.Add("@TotalRecords", dbType: DbType.String, direction: ParameterDirection.Output, size: 8000);
                var query = con.QueryMultiple(sql: "PRC_GETCUSTOMRECORD", P, commandType: System.Data.CommandType.StoredProcedure);

                rtnValues.rows = query.Read<ProductDataModel>().ToList();
                rtnValues.total = Convert.ToInt32(P.Get<string>("@TotalRecords"));
                //info.Success = true;

                con.Close();
            }
            catch (Exception ex)
            {

            }
            return rtnValues;
        }
        public static void whereClause(string columnName, string searchVal, ref string whereClause)
        {
            if (whereClause == "")
            {
                whereClause = columnName + " LIKE '%" + searchVal + "%'";
            }
            else
            {
                whereClause = whereClause + " OR " + columnName + " LIKE '%" + searchVal + "%'";
            }
        }

        public IActionResult AddProduct(int ID)
        {
            ProductWebModel model = new ProductWebModel();
            if (ID != 0)
            {
                string query = String.Format("SELECT * FROM TBL_PRODUCT WHERE ID = {0}", ID);
                var con = DbHandlerBase.GetConnection();
                ProductDataModel ProductDataModel = new ProductDataModel();
                con.Open();
                model = con.Query<ProductWebModel>(query).Single();
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult SaveProduct(ProductWebModel item)
        {
            if (!string.IsNullOrEmpty(item.PRODUCT) && !string.IsNullOrEmpty(item.DESCRIPTION))
            {
                ReturnInfoModel returnInfoModel = new ReturnInfoModel();
                char ACTION = 'I';
                if (item.ID != 0)
                {
                    ACTION = 'U';
                }
                try
                {
                    var con = DbHandlerBase.GetConnection();
                    con.Open();

                    var P = new DynamicParameters();
                    P.Add("@P_ID", item.ID);
                    P.Add("@P_PRODUCT", item.PRODUCT);
                    P.Add("@P_DESCRIPTION", item.DESCRIPTION);
                    P.Add("@P_ACTION", ACTION);
                    con.QueryMultiple(sql: "PRC_PRODUCT_IUD", P, commandType: System.Data.CommandType.StoredProcedure);
                    con.Close();
                    if (ACTION == 'I')
                    {
                        returnInfoModel.Message = "Added";
                    }
                    else if (ACTION == 'U')
                    {
                        returnInfoModel.Message = "Updated";

                    }
                    else
                    {
                        returnInfoModel.Message = "Deleted";
                    }
                    returnInfoModel.Success = true;
                }
                catch (Exception ex)
                {
                    returnInfoModel.Message = ex.Message;
                    returnInfoModel.Success = false;
                }
                return Json(returnInfoModel);
            }
            else
            {
                return Json(new ReturnInfoModel());
            }
        }

        public IActionResult DeleteProduct(int ID)
        {
            ReturnInfoModel returnInfoModel = new ReturnInfoModel();
            if (ID != 0)
            {
                try
                {
                    string query = String.Format("DELETE FROM TBL_PRODUCT WHERE ID = {0}", ID);
                    var con = DbHandlerBase.GetConnection();
                    con.Execute(query);
                    returnInfoModel.Message = "Deleted";
                    returnInfoModel.Success = true;
                }
                catch (Exception ex)
                {
                    returnInfoModel.Message = ex.Message;
                    returnInfoModel.Success = false;
                }

            }
            return Json(returnInfoModel);
        }
        public IActionResult ViewProduct(int ID)
        {
            ProductWebModel model = new ProductWebModel();
            if (ID != 0)
            {
                string query = String.Format("SELECT * FROM VW_PRODUCT WHERE ID = {0}", ID);
                var con = DbHandlerBase.GetConnection();
                ProductDataModel ProductDataModel = new ProductDataModel();
                con.Open();
                model = con.Query<ProductWebModel>(query).Single();
            }
            return View(model);
        }
    }
}
