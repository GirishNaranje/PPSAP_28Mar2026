using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using PPSAP.Common;
using PPSAP.DTO;
using PPSAP.SQLHelper;
using PPSAP.SQLHelper.DataAccessProvider;

namespace PPSAP.DAL
{
    public static class ExamQuestionDAL
    {
        public static int AddExamQuestion(List<ExamQuestionDTO> examQuestionList, ExamDTO examObj)
        {
            List<SelectedQuestion> questionList = new List<SelectedQuestion>();
            foreach (ExamQuestionDTO examQuestion in examQuestionList)
            {
                questionList.Add(new SelectedQuestion { ExamId = examQuestion.ExamId, QuestionId = examQuestion.QuestionId });
            }

            string tmpTable = "create table #question_selected (ID BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED (ID ASC), ExamId int, QuestionId int)";

            // Create a datatable that matches the temp table exactly. (WARNING: order of columns must match the order in the table)
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("ID", typeof(long)));
            table.Columns.Add(new DataColumn("ExamId", typeof(int)));
            table.Columns.Add(new DataColumn("QuestionId", typeof(int)));

            // Add prices in our list to our DataTable
            int id = 1;
            foreach (SelectedQuestion questionListitem in questionList)
            {
                DataRow row = table.NewRow();
                row["ID"] = id;
                row["ExamId"] = questionListitem.ExamId;
                row["QuestionId"] = questionListitem.QuestionId;
                table.Rows.Add(row);
                id++;
            }

            // Connect to DB
            string conString = SqlConnectionProvider.GetConnectionString(DataAccessType.Write);
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();

                // Execute the command to make a temp table
                SqlCommand cmd = new SqlCommand(tmpTable, con);
                cmd.ExecuteNonQuery();

                // BulkCopy the data in the DataTable to the temp table
                using (SqlBulkCopy bulk = new SqlBulkCopy(con))
                {
                    bulk.DestinationTableName = "#question_selected";
                    bulk.WriteToServer(table);
                }

                string mergeSql = string.Empty;
                if (examObj.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.SpacedRepetition))
                {
                    mergeSql = " DELETE FROM ExamQuestion WHERE ExamId = " + examObj.ExamId + " INSERT INTO ExamQuestion (ExamId,QuestionId) SELECT ExamId,QuestionId FROM #question_selected order by ID; ";
                }
                else
                {
                    // Now use the merge command to upsert from the temp table to the production table
                    mergeSql = "merge into ExamQuestion as Target " +
                                      "using #question_selected as Source " +
                                      "on " +
                                      "Target.ExamId=Source.ExamId " +
                                      "and Target.QuestionId = Source.QuestionId " +
                                      "when matched then " +
                                      "update set Target.QuestionId=Source.QuestionId " +
                                      "when not matched then " +
                                      "insert (ExamId,QuestionId) values (Source.ExamId,Source.QuestionId);";
                }

                cmd.CommandText = mergeSql;
                cmd.ExecuteNonQuery();

                // Clean up the temp table
                cmd.CommandText = "drop table #question_selected";
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return 1;
        }
    }

    internal class SelectedQuestion
    {
        public int ExamId { get; set; }

        public int QuestionId { get; set; }
    }
}
