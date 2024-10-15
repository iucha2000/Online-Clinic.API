using Online_Clinic.API.Enums;
using Online_Clinic.API.Exceptions;
using Online_Clinic.API.Interfaces;
using Online_Clinic.API.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Online_Clinic.API.Repositories.Oracle
{
    public class PKG_PATIENTS : PKG_BASE, IPatientRepository
    {
        public PKG_PATIENTS(IConfiguration configuration) : base(configuration)
        {
        }

        public void AddEntity(Patient entity)
        {
            Patient existingPatient = GetByEmail(entity.Email, throwIfNotFound: false);
            if(existingPatient != null)
            {
                throw new UserAlreadyExistsException("User with given email already exists");
            }

            existingPatient = GetByPersonalId(entity.Personal_Id, throwIfNotFound: false);
            if(existingPatient != null)
            {
                throw new UserAlreadyExistsException("User with given personalId already exists");
            }

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_PATIENTS.add_patient";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_firstname", OracleDbType.Varchar2).Value = entity.FirstName;
            cmd.Parameters.Add("v_lastname", OracleDbType.Varchar2).Value = entity.LastName;
            cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = entity.Email;
            cmd.Parameters.Add("v_password", OracleDbType.Varchar2).Value = entity.Password;
            cmd.Parameters.Add("v_personal_id", OracleDbType.Varchar2).Value = entity.Personal_Id;
            cmd.Parameters.Add("v_activationcode", OracleDbType.Int32).Value = entity.ActivationCode;
            cmd.Parameters.Add("v_role", OracleDbType.Int32).Value = entity.Role;

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void UpdateEntity(int id, Patient entity)
        {
            Patient existingPatient = GetEntity(id);

            existingPatient = GetByEmail(entity.Email, throwIfNotFound: false);
            if (existingPatient != null)
            {
                throw new UserAlreadyExistsException("User with given email already exists");
            }

            existingPatient = GetByPersonalId(entity.Personal_Id, throwIfNotFound: false);
            if (existingPatient != null)
            {
                throw new UserAlreadyExistsException("User with given personalId already exists");
            }

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_PATIENTS.update_patient";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("v_firstname", OracleDbType.Varchar2).Value = entity.FirstName;
            cmd.Parameters.Add("v_lastname", OracleDbType.Varchar2).Value = entity.LastName;
            cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = entity.Email;
            cmd.Parameters.Add("v_password", OracleDbType.Varchar2).Value = entity.Password;
            cmd.Parameters.Add("v_personal_id", OracleDbType.Varchar2).Value = entity.Personal_Id;
            cmd.Parameters.Add("v_activationcode", OracleDbType.Int32).Value = entity.ActivationCode;
            cmd.Parameters.Add("v_role", OracleDbType.Int32).Value = entity.Role;

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void DeleteEntity(int id)
        {
            Patient existingPatient = GetEntity(id);

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_PATIENTS.delete_patient";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = id;

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public Patient GetEntity(int id, bool throwIfNotFound = true)
        {
            Patient patient = null;

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_PATIENTS.get_patient_by_id";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                patient = new Patient()
                {
                    Id = id,
                    FirstName = reader["firstname"].ToString(),
                    LastName = reader["lastname"].ToString(),
                    Email = reader["email"].ToString(),
                    Password = reader["password"].ToString(),
                    Personal_Id = reader["personal_id"].ToString(),
                    ActivationCode = int.Parse(reader["activationcode"].ToString()),
                    Role = (Role)int.Parse(reader["role"].ToString())
                };
            }
            else if(throwIfNotFound)
            {
                throw new UserNotFoundException($"User with id:'{id}' does not exist");
            }

            conn.Close();

            return patient;
        }

        public List<Patient> GetEntities()
        {
            List<Patient> patients = new List<Patient>();

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_PATIENTS.get_all_patients";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Patient patient = new Patient()
                {
                    Id = int.Parse(reader["id"].ToString()),
                    FirstName = reader["firstname"].ToString(),
                    LastName = reader["lastname"].ToString(),
                    Email = reader["email"].ToString(),
                    Password = reader["password"].ToString(),
                    Personal_Id = reader["personal_id"].ToString(),
                    ActivationCode = int.Parse(reader["activationcode"].ToString()),
                    Role = (Role)int.Parse(reader["role"].ToString())
                };

                patients.Add(patient);
            }

            conn.Close();
            return patients;
        }

        public Patient GetByEmail(string email, bool throwIfNotFound = true)
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
                    FirstName = reader["firstname"].ToString(),
                    LastName = reader["lastname"].ToString(),
                    Email = reader["email"].ToString(),
                    Password = reader["password"].ToString(),
                    Personal_Id = reader["personal_id"].ToString(),
                    ActivationCode = int.Parse(reader["activationcode"].ToString()),
                    Role = (Role)int.Parse(reader["role"].ToString())
                };
            }
            else if (throwIfNotFound)
            {
                throw new UserNotFoundException($"User with email:'{email}' does not exist");
            }

            conn.Close();

            return patient;
        }

        public Patient GetByPersonalId(string personalId, bool throwIfNotFound = true)
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
                    FirstName = reader["firstname"].ToString(),
                    LastName = reader["lastname"].ToString(),
                    Email = reader["email"].ToString(),
                    Password = reader["password"].ToString(),
                    Personal_Id = reader["personal_id"].ToString(),
                    ActivationCode = int.Parse(reader["activationcode"].ToString()),
                    Role = (Role)int.Parse(reader["role"].ToString())
                };
            }
            else if (throwIfNotFound)
            {
                throw new UserNotFoundException($"User with personalId:'{personalId}' does not exist");
            }

            conn.Close();

            return patient;
        }
    }
}
