using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PPSAP.Common;
using PPSAP.DTO;
using PPSAP.SQLHelper;
using PPSAP.SQLHelper.DataAccessProvider;

namespace PPSAP.DAL
{
    public class UserViewDAL
    {
        public static QuestionDetails GetQuestionDetailsByQuestionid(int? questionId)
        {
            QuestionDetails questionDetail = new QuestionDetails();
            SqlParameter[] arrSqlParameter =
            {
                new SqlParameter("@QuestionId", Convert.ToInt32(questionId)),
               };
            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "PPSAP_GetQuestionDetailsByQuestionid", arrSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    questionDetail.questionId = Convert.ToInt32(objSqlDataReader["questionid"]);
                    object prevQuestionIdObj = objSqlDataReader["prevQuestionId"];
                    questionDetail.previousId = prevQuestionIdObj is DBNull ? -1 : Convert.ToInt32(objSqlDataReader["prevQuestionId"]);
                    object nextQuestionIdObj = objSqlDataReader["nextQuestionId"];
                    questionDetail.nextId = nextQuestionIdObj is DBNull ? -2 : Convert.ToInt32(objSqlDataReader["nextQuestionId"]);
                    object serialNumberObj = objSqlDataReader["serialNumber"];
                    questionDetail.serialNumber = serialNumberObj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["serialNumber"]);
                    object stemObj = objSqlDataReader["Stem"];
                    questionDetail.questionText = stemObj is DBNull ? null : Convert.ToString(objSqlDataReader["Stem"]);
                    //object friendlyidobj = objSqlDataReader["Friendlyid"];
                    //questionDetail.FriendlyId = friendlyidobj is DBNull ? 0 : Convert.ToInt32(objSqlDataReader["Friendlyid"]);
                }

                objSqlDataReader.Close();

                List<QuestionImage> questionImageList = new List<QuestionImage>();
                questionImageList = AssessmentDAL.GetQuestionImageList(questionId);
                questionDetail.questionImage = questionImageList;
                return questionDetail;
            }
        }

        public static int GetQuestionIdByFriendlyid(int? friendlyid, SqlConnection gConnection = null)
        {
            int questionid = 0;
            SqlConnection connection = gConnection;
            if (gConnection == null)
            {
                connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));

            }
            string sqlQueryChoice = string.Empty;

            sqlQueryChoice = "Select Questionid FROM Question WHERE QuestionId= " + @friendlyid + ";";

            if (gConnection == null)
            {
                connection.Open();
            }

            SqlCommand cmd = new SqlCommand(sqlQueryChoice, connection);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    // Read advances to the next row.
                    while (reader.Read())
                    {
                        questionid = reader.GetInt32(reader.GetOrdinal("Questionid"));
                    }

                    reader.Close();
                }
            }
            if (gConnection == null)
            {
                connection.Close();
            }
            return questionid;
        }

        public static List<Choice> GetChoiceListBYQuestionId(int? questionId, SqlConnection gConnection = null)
        {
            List<Choice> choiceList = new List<Choice>();
            SqlConnection connection = gConnection;
            if (gConnection == null)
            {
                connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));

            }
            string sqlQueryChoice = string.Empty;

            sqlQueryChoice = "Select Choice_Id As ID, ChoiceText as Text FROM QuestionChoice WHERE QuestionId= " + @questionId + " ORDER BY NEWID()";

            if (gConnection == null)
            {
                connection.Open();
            }
            SqlCommand cmd = new SqlCommand(sqlQueryChoice, connection);
            int sequenceNo = 64;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    // Read advances to the next row.
                    while (reader.Read())
                    {
                        Choice choice = new Choice();
                        choice.choiceId = reader.GetInt32(reader.GetOrdinal("ID"));
                        object textObj = reader["Text"];
                        choice.choiceText = textObj is DBNull ? null : reader.GetString(reader.GetOrdinal("Text"));
                        sequenceNo++;
                        choice.choiceSequence = (char)sequenceNo;
                        choice.ChoiceImage = AssessmentDAL.GetChoiceImageList(choice.choiceId);
                        choiceList.Add(choice);
                    }

                    reader.Close();
                }
            }
            if (gConnection == null)
            {
                connection.Close();
            }
            return choiceList;
        }
    }
}
