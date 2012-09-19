#region Using Statements
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
#endregion

namespace Apollo
{

    /// <summary>TBD</summary>
    public abstract class DataIO
    {

        #region CommandType enumeration
        private enum CommandType
        {
            Sql,
            StoredProc,
        }
        #endregion

        #region QueryResultType enumeration
        private enum QueryResultType
        {
            DataSet,
            DataReader,
            Scalar,
            Null,
        }
        #endregion

        public enum MultiValueParamType
        {
            Int,
            String,
        }

        #region ConvertTextTypeToSqlType method
        /// <summary>Converts the plain-text SQL Server Parameter type to a SqlDbType enum value</summary>
        /// <param name="paramType">The plain-text SQL Server Parameter type</param>
        /// <returns>SqlDbType enum representation of parameter type</returns>
        private static SqlDbType ConvertTextTypeToSqlType(string paramType)
        {
            switch (paramType.ToLower(CultureInfo.CurrentCulture))
            {
                case "date":
                    return SqlDbType.Date;
                case "datetime":
                    return SqlDbType.DateTime;
                case "int":
                    return SqlDbType.Int;
                case "ntext":
                    return SqlDbType.NText;
                case "varchar":
                    return SqlDbType.VarChar;
                case "bigint":
                    return SqlDbType.BigInt;
                case "numeric":
                    return SqlDbType.Decimal;
                case "decimal":
                    return SqlDbType.Decimal;
                case "char":
                    return SqlDbType.Char;
                case "structured":
                    return SqlDbType.Structured;
                case "bit":
                    return SqlDbType.Bit;
                default:
                    throw new ArgumentOutOfRangeException("paramType", "Unexpected/unsupported paramType: " + paramType + " encountered.");
            }
        }
        #endregion

        #region ExecuteActionQuery method (queryCommand)
        /// <summary>Executes an Insert, Update, or Delete Query</summary>
        /// <param name="queryCommand">The SqlCommand to execute</param>
        public static void ExecuteActionQuery(SqlCommand queryCommand)
        {
            //Do not handle exceptions at this level.
            //The caller should trap exceptions by wrapping a call to this method in a try/catch block
            ExecuteQuery(queryCommand, QueryResultType.Null);
        }
        #endregion

        #region ExecuteActionQuery method (queryCommand, conn)
        /// <summary>Executes an Insert, Update, or Delete Query using the Command and Connection provided</summary>
        /// <param name="queryCommand">The SqlCommand to execute</param>
        /// <param name="conn">The SqlConnection to execute the Command on</param>
        public static void ExecuteActionQuery(SqlCommand queryCommand, SqlConnection conn)
        {
            //Do not handle exceptions at this level.
            //The caller should trap exceptions by wrapping a call to this method in a try/catch block
            ExecuteQuery(queryCommand, QueryResultType.Null, conn);
        }
        #endregion

        #region ExecuteDataReaderQuery method (queryCommand)
        /// <summary>Executes a SELECT query and returns a DataReader</summary>
        /// <param name="queryCommand">The SqlCommand to execute</param>
        /// <returns>An open SqlDataReader to retrieve query results</returns>
        public static SqlDataReader ExecuteDataReaderQuery(SqlCommand queryCommand)
        {
            //Do not handle exceptions at this level.
            //The caller should trap exceptions by wrapping a call to this method in a try/catch block
            return (SqlDataReader)ExecuteQuery(queryCommand, QueryResultType.DataReader);
        }
        #endregion

        #region ExecuteDataReaderQuery method (queryCommand, conn)
        /// <summary>Executes a SELECT query and returns a DataReader using the Connection provided</summary>
        /// <param name="queryCommand">The SqlCommand to execute</param>
        /// <param name="conn">The SqlConnection to execute the Command on</param>
        /// <returns>An open SqlDataReader to retrieve query results</returns>
        public static SqlDataReader ExecuteDataReaderQuery(SqlCommand queryCommand, SqlConnection conn)
        {
            //Do not handle exceptions at this level.
            //The caller should trap exceptions by wrapping a call to this method in a try/catch block
            return (SqlDataReader)ExecuteQuery(queryCommand, QueryResultType.DataReader, conn);
        }
        #endregion

        #region ExecuteDataSetQuery method (queryCommand)
        /// <summary>Executes a SELECT query and returns a DataSet containing the </summary>
        /// <param name="queryCommand">The SqlCommand object to execute</param>
        /// <returns>A DataSet object populated with the results of the SqlCommand</returns>
        public static DataSet ExecuteDataSetQuery(SqlCommand queryCommand)
        {
            //Do not handle exceptions at this level.
            //The caller should trap exceptions by wrapping a call to this method in a try/catch block
            return (DataSet)ExecuteQuery(queryCommand, QueryResultType.DataSet);
        }
        #endregion

        #region ExecuteDataSetQuery method (queryCommand, conn)
        /// <summary>Executes a SELECT query and returns a DataSet using the Connection provided</summary>
        /// <param name="queryCommand">The SqlCommand object to execute</param>
        /// <param name="conn">The SqlConnection to execute the Command on</param>
        /// <returns>A DataSet object populated with the results of the SqlCommand</returns>
        public static DataSet ExecuteDataSetQuery(SqlCommand queryCommand, SqlConnection conn)
        {
            //Do not handle exceptions at this level.
            //The caller should trap exceptions by wrapping a call to this method in a try/catch block
            return (DataSet)ExecuteQuery(queryCommand, QueryResultType.DataSet, conn);
        }
        #endregion

        #region ExecuteQuery method (queryCommand, resultType)
        /// <summary>
        ///     Executes the query provided by the SqlCommand object.
        ///     Returns an object corresponding to QueryResultType
        /// </summary>
        /// <param name="queryCommand">SqlCommand object with query to be executed</param>
        /// <param name="resultType">QueryResultType to return</param>
        /// <returns>
        ///     System.Object cast to QueryResultType value.
        ///     Return types are: SqlDataReader, DataSet, System.Int, System.Object
        /// </returns>
        private static object ExecuteQuery(SqlCommand queryCommand, QueryResultType resultType)
        {
            using (SqlConnection conn = GetConnection)
            {
                return ExecuteQuery(queryCommand, resultType, conn);
            }
        }
        #endregion

        #region ExecuteQuery method (queryCommand, resultType, conn)
        /// <summary>
        ///     Executes the query provided by the SqlCommand object.
        ///     Returns an object corresponding to QueryResultType
        /// </summary>
        /// <param name="queryCommand">SqlCommand object with query to be executed</param>
        /// <param name="resultType">QueryResultType to return</param>
        /// <param name="conn">The SqlConnection to execute the Command on</param>
        /// <returns>
        ///     System.Object cast to QueryResultType value.
        ///     Return types are: SqlDataReader, DataSet, System.Int, System.Object
        /// </returns>
        private static object ExecuteQuery(SqlCommand queryCommand, QueryResultType resultType, SqlConnection conn)
        {
            //Make sure we don't try to open an already-open connection
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            queryCommand.Connection = conn;
            switch (resultType)
            {
                case QueryResultType.DataReader:
                    return queryCommand.ExecuteReader();
                case QueryResultType.DataSet:
                    {
                        DataSet ds = new DataSet();
                        ds.Locale = CultureInfo.CurrentCulture;
                        using (SqlDataAdapter da = new SqlDataAdapter())
                        {
                            da.SelectCommand = queryCommand;
                            da.Fill(ds);
                        }
                        return ds;
                    }
                case QueryResultType.Null:
                    return queryCommand.ExecuteNonQuery();
                case QueryResultType.Scalar:
                    return queryCommand.ExecuteScalar();
                default:
                    throw new ArgumentOutOfRangeException("resultType", "resultType is not a valid QueryResultType");
            }
        }
        #endregion

        #region ExecuteScalarQuery method (queryCommand)
        /// <summary>Executes a single-result query</summary>
        /// <param name="queryCommand">The SqlCommand object to execute</param>
        /// <returns>Returns an object containing the scalar result</returns>
        public static object ExecuteScalarQuery(SqlCommand queryCommand)
        {
            //Do not handle exceptions at this level.
            //The caller should trap exceptions by wrapping a call to this method in a try/catch block
            return ExecuteQuery(queryCommand, QueryResultType.Scalar);
        }
        #endregion

        #region ExecuteScalarQuery method (queryCommand, conn)
        /// <summary>Executes a single-result query using the Connection provided</summary>
        /// <param name="queryCommand">The SqlCommand object to execute</param>
        /// <param name="conn">The SqlConnection to execute the Command on</param>
        /// <returns>Returns an object containing the scalar result</returns>
        public static object ExecuteScalarQuery(SqlCommand queryCommand, SqlConnection conn)
        {
            //Do not handle exceptions at this level.
            //The caller should trap exceptions by wrapping a call to this method in a try/catch block
            return ExecuteQuery(queryCommand, QueryResultType.Scalar, conn);
        }
        #endregion

        #region GetCommand method
        /// <summary>
        ///     Builds a SqlCommand object using the commandText,
        ///     commandType, and command parameters
        /// </summary>
        /// <param name="commandText">The SQL statement that the command will execute</param>
        /// <param name="commandType">
        ///     The type of command. Valid choices are:
        ///         CommandType.Sql
        ///         CommandType.StoredProc
        /// </param>
        /// <param name="commandParams">A Hashtable object containing a key/value list of parameters</param>
        /// <returns>A SqlCommand object populated with command text, type, and parameters</returns>
        private static SqlCommand GetCommand(string commandText, CommandType commandType, Hashtable commandParams)
        {
            SqlCommand queryCommand = new SqlCommand();
            queryCommand.CommandTimeout = 200;
            switch (commandType)
            {
                case CommandType.Sql:
                    queryCommand.CommandText = commandText;
                    return queryCommand;
                case CommandType.StoredProc:
                    queryCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    queryCommand.CommandText = commandText;
                    if (commandParams != null)
                    {
                        foreach (DictionaryEntry paramName in commandParams)
                        {
                            queryCommand.Parameters.Add(Convert.ToString(paramName.Key, CultureInfo.CurrentCulture), GetParamType(commandText, Convert.ToString(paramName.Key, CultureInfo.CurrentCulture)));
                            queryCommand.Parameters[Convert.ToString(paramName.Key, CultureInfo.CurrentCulture)].Value = (paramName.Value == null) ? DBNull.Value : paramName.Value;
                        }
                    }
                    return queryCommand;
                default:
                    return null;
            }
        }
        #endregion

        #region GetCommandFromSql method
        /// <summary>Creates a SqlCommand object from a provided SQL statement</summary>
        /// <param name="sql">The SQL Statement to use for the SqlCommand.CommandText property</param>
        /// <returns>A SqlCommand object to be executed</returns>
        public static SqlCommand GetCommandFromSql(string sql)
        {
            return GetCommand(sql, CommandType.Sql, null);
        }
        #endregion

        #region GetCommandFromStoredProc method (storedProcName)
        /// <summary>Creates a SqlCommand object from a pre-defined Stored Procedure</summary>
        /// <param name="storedProcName">The name of the Stored Procedure to use</param>
        /// <returns>A SqlCommand object to be executed</returns>
        public static SqlCommand GetCommandFromStoredProc(string storedProcName)
        {
            return GetCommandFromStoredProc(storedProcName, null);
        }
        #endregion

        #region GetCommandFromStoredProc method (storedProcName, commandParams)
        /// <summary>
        ///     Creates a SqlCommand object from a pre-defined
        ///     Stored Procedure and parameter list
        /// </summary>
        /// <param name="storedProcName">The name of the Stored Procedure to use</param>
        /// <param name="commandParams">The Hashtable object containing the key/value list of parameters</param>
        /// <returns>A SqlCommand object to be executed</returns>
        public static SqlCommand GetCommandFromStoredProc(string storedProcName, Hashtable commandParams)
        {
            return GetCommand(storedProcName, CommandType.StoredProc, commandParams);
        }
        #endregion

        #region GetConnection property
        /// <summary>Opens a connection to the application Database.</summary>
        /// <value>TBD</value>
        public static SqlConnection GetConnection
        {
            get
            {
                return new SqlConnection(WebCommon.ConnectionString);
            }
        }
        #endregion

        #region GetDataRowValue method
        /// <summary>TBD</summary>
        /// <param name="row">TBD</param>
        /// <param name="columnName">TBD</param>
        /// <param name="defaultValue">TBD</param>
        /// <returns>TBD</returns>
        public static object GetDataRowValue(DataRow row, string columnName, object defaultValue)
        {
            if (row == null)
            {
                return defaultValue;
            }
            if (row[columnName] == DBNull.Value)
            {
                return defaultValue;
            }
            return row[columnName];
        }
        #endregion

        private static DataTable GetParamTable(MultiValueParamType paramType)
        {
            DataTable paramTable = new DataTable("MultiValueParam");
            switch (paramType)
            {
                case MultiValueParamType.Int:
                    paramTable.Columns.Add(new DataColumn("INT_PARAM"));
                    break;
                case MultiValueParamType.String:
                    paramTable.Columns.Add(new DataColumn("VARCHAR_PARAM"));
                    break;
                default: throw new ArgumentOutOfRangeException("paramType");
            }
            return paramTable;
        }

        public static DataTable ConvertArrayToStructuredType(MultiValueParamType paramType, object[] objectParams)
        {
            DataTable paramTable = GetParamTable(paramType);
            foreach (object param in objectParams)
            {
                if (param == null)
                {
                    continue;
                }
                if (paramType == MultiValueParamType.Int)
                {
                    paramTable.Rows.Add(Convert.ToInt32(param));
                }
                else if (paramType == MultiValueParamType.String)
                {
                    paramTable.Rows.Add(Convert.ToString(param));
                }
                else
                {
                    throw new ArgumentOutOfRangeException("paramType");
                }
            }
            return paramTable;
        }

        #region GetDataSource method
        /// <summary>TBD</summary>
        /// <param name="sourceCommand">TBD</param>
        /// <returns>TBD</returns>
        private static SqlDataSource GetDataSource(string sourceCommand)
        {
            return new SqlDataSource(WebCommon.ConnectionString, sourceCommand);
        }
        #endregion

        #region GetParamType method
        /// <summary>
        ///     Performs a lookup against the StoredProcParams.xml file
        ///     using the Stored Proc Name and Param Name to
        ///     obtain the SqlDbType of the parameter
        /// </summary>
        /// <param name="storedProcName">The name of the stored procedure being executed</param>
        /// <param name="paramName">The name of the current parameter</param>
        /// <returns>Returns the expected SqlDbType of the parameter</returns>
        private static SqlDbType GetParamType(string storedProcName, string paramName)
        {
            string paramTypeXPathQuery = @"/storedprocedures/storedproc[name = ""{0}""]/params/param[@name = ""{1}""]/@type";
            XmlDocument xmlDoc = new XmlDocument();
            //Updated this to allow usage in multi-threaded environment:
            //xmlDoc.Load(HttpContext.Current.Server.MapPath("~/App_Data/StoredProcParams.xml"));
            xmlDoc.Load(HostingEnvironment.MapPath(HostingEnvironment.ApplicationVirtualPath + "/App_Data/StoredProcParams.xml"));
            string paramType = xmlDoc.SelectSingleNode(string.Format(CultureInfo.CurrentCulture, paramTypeXPathQuery, storedProcName.ToUpper(CultureInfo.CurrentCulture), paramName.ToUpper(CultureInfo.CurrentCulture))).Value;
            return ConvertTextTypeToSqlType(paramType);
        }
        #endregion

    }

}
