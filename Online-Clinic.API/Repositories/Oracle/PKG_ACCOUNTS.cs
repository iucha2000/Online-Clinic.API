
using Online_Clinic.API.Enums;
using Online_Clinic.API.Exceptions;
using Online_Clinic.API.Interfaces;
using Online_Clinic.API.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Online_Clinic.API.Repositories.Oracle
{
    public class PKG_ACCOUNTS : PKG_BASE, IAccountRepository
    {
        public PKG_ACCOUNTS(IConfiguration configuration) : base(configuration)
        {
        }

        public Account ValidateUser(string email, string password)
        {
            var patient = GetPatientByEmail(email, false);
            if (patient != null)
            {
                ValidatePassword(patient.Password, password);
                return patient;
            }

            var doctor = GetDoctorByEmail(email, false);
            if (doctor != null)
            {
                ValidatePassword(doctor.Password, password);
                return doctor;
            }

            throw new InvalidCredentialsException("Invalid credentials");
        }

        public void ValidatePassword(string storedPassword, string providedPassword)
        {
            if (storedPassword != providedPassword)
            {
                throw new InvalidCredentialsException("Invalid credentials");
            }
        }

        public bool EmailExists(string email)
        {
            Patient existingPatient = GetPatientByEmail(email, false);
            if (existingPatient != null)
            {
                return true;
            }

            Doctor existingDoctor = GetDoctorByEmail(email, false);
            if (existingDoctor != null)
            {
                return true;
            }

            return false;
        }

        public bool PersonalIdExists(string personalId)
        {
            Patient existingPatient = GetPatientByPersonalId(personalId, false);
            if(existingPatient != null)
            {
                return true;
            }

            Doctor existingDoctor = GetDoctorByPersonalId(personalId, false);
            if (existingDoctor != null)
            {
                return true;
            }

            return false;
        }

        public Patient GetPatientByEmail(string email, bool throwIfNotFound = true)
        {
            Patient patient = null;

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_PATIENTS.get_patient_by_email";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = email;
            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                patient = new Patient()
                {
                    Id = int.Parse(reader["id"].ToString()),
                    FirstName = reader["firstname"] != DBNull.Value ? reader["firstname"].ToString() : null,
                    LastName = reader["lastname"] != DBNull.Value ? reader["lastname"].ToString() : null,
                    Email = reader["email"] != DBNull.Value ? reader["email"].ToString() : null,
                    Password = reader["password"] != DBNull.Value ? reader["password"].ToString() : null,
                    Personal_Id = reader["personal_id"] != DBNull.Value ? reader["personal_id"].ToString() : null,
                    Role = reader["role"] != DBNull.Value ? (Role?)Convert.ToInt32(reader["role"]) : null
                };
            }
            else if (throwIfNotFound)
            {
                throw new UserNotFoundException($"User with email:'{email}' does not exist");
            }

            conn.Close();

            return patient;
        }

        public Doctor GetDoctorByEmail(string email, bool throwIfNotFound = true)
        {
            Doctor doctor = null;

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_DOCTORS.get_doctor_by_email";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = email;
            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                doctor = new Doctor()
                {
                    Id = int.Parse(reader["id"].ToString()),
                    FirstName = reader["firstname"] != DBNull.Value ? reader["firstname"].ToString() : null,
                    LastName = reader["lastname"] != DBNull.Value ? reader["lastname"].ToString() : null,
                    Email = reader["email"] != DBNull.Value ? reader["email"].ToString() : null,
                    Password = reader["password"] != DBNull.Value ? reader["password"].ToString() : null,
                    Personal_Id = reader["personal_id"] != DBNull.Value ? reader["personal_id"].ToString() : null,
                    Role = reader["role"] != DBNull.Value ? (Role?)Convert.ToInt32(reader["role"]) : null,
                    Category = reader["category"] != DBNull.Value ? (Category?)Convert.ToInt32(reader["category"]) : null,
                    Rating = reader["rating"] != DBNull.Value ? (int?)Convert.ToInt32(reader["rating"]) : null
                };
            }
            else if (throwIfNotFound)
            {
                throw new UserNotFoundException($"Doctor with email:'{email}' does not exist");
            }

            conn.Close();

            return doctor;
        }

        public Patient GetPatientByPersonalId(string personalId, bool throwIfNotFound = true)
        {
            Patient patient = null;

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_PATIENTS.get_patient_by_personal_id";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_personal_id", OracleDbType.Varchar2).Value = personalId;
            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                patient = new Patient()
                {
                    Id = int.Parse(reader["id"].ToString()),
                    FirstName = reader["firstname"] != DBNull.Value ? reader["firstname"].ToString() : null,
                    LastName = reader["lastname"] != DBNull.Value ? reader["lastname"].ToString() : null,
                    Email = reader["email"] != DBNull.Value ? reader["email"].ToString() : null,
                    Password = reader["password"] != DBNull.Value ? reader["password"].ToString() : null,
                    Personal_Id = reader["personal_id"] != DBNull.Value ? reader["personal_id"].ToString() : null,
                    Role = reader["role"] != DBNull.Value ? (Role?)Convert.ToInt32(reader["role"]) : null
                };
            }
            else if (throwIfNotFound)
            {
                throw new UserNotFoundException($"User with personalId:'{personalId}' does not exist");
            }

            conn.Close();

            return patient;
        }

        public Doctor GetDoctorByPersonalId(string personalId, bool throwIfNotFound = true)
        {
            Doctor doctor = null;

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_DOCTORS.get_doctor_by_personal_id";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_personal_id", OracleDbType.Varchar2).Value = personalId;
            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                doctor = new Doctor()
                {
                    Id = int.Parse(reader["id"].ToString()),
                    FirstName = reader["firstname"] != DBNull.Value ? reader["firstname"].ToString() : null,
                    LastName = reader["lastname"] != DBNull.Value ? reader["lastname"].ToString() : null,
                    Email = reader["email"] != DBNull.Value ? reader["email"].ToString() : null,
                    Password = reader["password"] != DBNull.Value ? reader["password"].ToString() : null,
                    Personal_Id = reader["personal_id"] != DBNull.Value ? reader["personal_id"].ToString() : null,
                    Role = reader["role"] != DBNull.Value ? (Role?)Convert.ToInt32(reader["role"]) : null,
                    Category = reader["category"] != DBNull.Value ? (Category?)Convert.ToInt32(reader["category"]) : null,
                    Rating = reader["rating"] != DBNull.Value ? (int?)Convert.ToInt32(reader["rating"]) : null
                };
            }
            else if (throwIfNotFound)
            {
                throw new UserNotFoundException($"Doctor with personalId:'{personalId}' does not exist");
            }

            conn.Close();

            return doctor;
        }

        public void UpdateUserPassword(string email, string password)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_PATIENTS.update_user_password";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = email;
            cmd.Parameters.Add("v_password", OracleDbType.Varchar2).Value = password;

            cmd.ExecuteNonQuery();

            conn.Close();
        }
    }
}
