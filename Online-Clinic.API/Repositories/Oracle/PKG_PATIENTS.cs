using Online_Clinic.API.Enums;
using Online_Clinic.API.Exceptions;
using Online_Clinic.API.Interfaces;
using Online_Clinic.API.Models;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace Online_Clinic.API.Repositories.Oracle
{
    public class PKG_PATIENTS : PKG_BASE, IPatientRepository
    {
        private readonly IAccountRepository _accountRepository;

        public PKG_PATIENTS(IConfiguration configuration, IAccountRepository accountRepository) : base(configuration)
        {
            _accountRepository = accountRepository;
        }

        public int AddEntity(Patient entity)
        {
            if (_accountRepository.EmailExists(entity.Email))
            {
                throw new UserAlreadyExistsException("User with given email already exists");
            }

            if (_accountRepository.PersonalIdExists(entity.Personal_Id))
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

            cmd.Parameters.Add("v_firstname", OracleDbType.Varchar2).Value = entity.FirstName ?? (object)DBNull.Value;
            cmd.Parameters.Add("v_lastname", OracleDbType.Varchar2).Value = entity.LastName ?? (object)DBNull.Value;
            cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = entity.Email ?? (object)DBNull.Value;
            cmd.Parameters.Add("v_password", OracleDbType.Varchar2).Value = entity.Password ?? (object)DBNull.Value;
            cmd.Parameters.Add("v_personal_id", OracleDbType.Varchar2).Value = entity.Personal_Id ?? (object)DBNull.Value;
            cmd.Parameters.Add("v_role", OracleDbType.Int32).Value = entity.Role.HasValue ? (int)entity.Role.Value : (object)DBNull.Value;

            OracleParameter generatedIdParam = new OracleParameter("v_generated_id", OracleDbType.Int32);
            generatedIdParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(generatedIdParam);

            cmd.ExecuteNonQuery();

            int generatedId = Convert.ToInt32(((OracleDecimal)generatedIdParam.Value).ToInt32());
            entity.Id = generatedId;

            conn.Close();

            return generatedId;
        }

        public void UpdateEntity(int id, Patient entity)
        {
            if (_accountRepository.EmailExists(entity.Email))
            {
                throw new UserAlreadyExistsException("User with given email already exists");
            }

            if (_accountRepository.PersonalIdExists(entity.Personal_Id))
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
            cmd.Parameters.Add("v_firstname", OracleDbType.Varchar2).Value = entity.FirstName ?? (object)DBNull.Value;
            cmd.Parameters.Add("v_lastname", OracleDbType.Varchar2).Value = entity.LastName ?? (object)DBNull.Value;
            cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = entity.Email ?? (object)DBNull.Value;
            cmd.Parameters.Add("v_password", OracleDbType.Varchar2).Value = entity.Password ?? (object)DBNull.Value;
            cmd.Parameters.Add("v_personal_id", OracleDbType.Varchar2).Value = entity.Personal_Id ?? (object)DBNull.Value;
            cmd.Parameters.Add("v_role", OracleDbType.Int32).Value = entity.Role.HasValue ? (int)entity.Role.Value : (object)DBNull.Value;

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
                    FirstName = reader["firstname"] != DBNull.Value ? reader["firstname"].ToString() : null,
                    LastName = reader["lastname"] != DBNull.Value ? reader["lastname"].ToString() : null,
                    Email = reader["email"] != DBNull.Value ? reader["email"].ToString() : null,
                    Password = reader["password"] != DBNull.Value ? reader["password"].ToString() : null,
                    Personal_Id = reader["personal_id"] != DBNull.Value ? reader["personal_id"].ToString() : null,
                    Role = reader["role"] != DBNull.Value ? (Role?)Convert.ToInt32(reader["role"]) : null
                };
            }
            else if(throwIfNotFound)
            {
                throw new UserNotFoundException($"Patient with id:'{id}' does not exist");
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
                    FirstName = reader["firstname"] != DBNull.Value ? reader["firstname"].ToString() : null,
                    LastName = reader["lastname"] != DBNull.Value ? reader["lastname"].ToString() : null,
                    Email = reader["email"] != DBNull.Value ? reader["email"].ToString() : null,
                    Password = reader["password"] != DBNull.Value ? reader["password"].ToString() : null,
                    Personal_Id = reader["personal_id"] != DBNull.Value ? reader["personal_id"].ToString() : null,
                    Role = reader["role"] != DBNull.Value ? (Role?)Convert.ToInt32(reader["role"]) : null
                };

                patients.Add(patient);
            }

            conn.Close();
            return patients;
        }

        public Patient GetByEmail(string email)
        {
            return _accountRepository.GetPatientByEmail(email, true);
        }

        public Patient GetByPersonalId(string personalId)
        {
            return _accountRepository.GetPatientByPersonalId(personalId, true);
        }
    }
}
