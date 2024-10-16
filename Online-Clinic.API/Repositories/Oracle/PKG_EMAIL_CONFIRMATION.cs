using Online_Clinic.API.Exceptions;
using Online_Clinic.API.Interfaces;
using Online_Clinic.API.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Online_Clinic.API.Repositories.Oracle
{
    public class PKG_EMAIL_CONFIRMATION : PKG_BASE, IEmailConfirmationRepository
    {
        public PKG_EMAIL_CONFIRMATION(IConfiguration configuration) : base(configuration)
        {
        }

        public void AddToDatabase(EmailConfirmation emailConfirmation)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_EMAIL_CONFIRMATIONS.add_confirmation";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = emailConfirmation.Email;
            cmd.Parameters.Add("v_code", OracleDbType.Int32).Value = emailConfirmation.Code;
            cmd.Parameters.Add("v_expirytime", OracleDbType.Date).Value = emailConfirmation.ExpiryTime;

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public EmailConfirmation GetFromDatabaseByEmail(string email)
        {
            EmailConfirmation emailConfirmation = null;

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_EMAIL_CONFIRMATIONS.get_confirmation_by_email";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = email;
            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            reader.Read();
            if (reader.HasRows)
            {
                emailConfirmation = new EmailConfirmation()
                {
                    Email = reader["email"].ToString(),
                    Code = int.Parse(reader["code"].ToString()),
                    ExpiryTime = Convert.ToDateTime(reader["expirytime"]),
                };
            }
            else
            {
                throw new UserNotFoundException($"Confirmation for mail:'{email}' does not exist");
            }

            conn.Close();

            return emailConfirmation;
        }
    }
}
