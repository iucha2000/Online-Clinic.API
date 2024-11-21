using Online_Clinic.API.Enums;
using Online_Clinic.API.Exceptions;
using Online_Clinic.API.Interfaces;
using Online_Clinic.API.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;
using Oracle.ManagedDataAccess.Types;
using System.Numerics;
using System.Reflection.PortableExecutable;

namespace Online_Clinic.API.Repositories.Oracle
{
    public class PKG_DOCTORS : PKG_BASE, IDoctorRepository
    {
        private readonly IAccountRepository _accountRepository;

        public PKG_DOCTORS(IConfiguration configuration, IAccountRepository accountRepository) : base(configuration)
        {
            _accountRepository = accountRepository;
        }

        public void AddEntity(Doctor entity)
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
            cmd.CommandText = "olerning.PKG_IURI_DOCTORS.add_doctor";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_firstname", OracleDbType.Varchar2).Value = entity.FirstName ?? (object)DBNull.Value;
            cmd.Parameters.Add("v_lastname", OracleDbType.Varchar2).Value = entity.LastName ?? (object)DBNull.Value;
            cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = entity.Email ?? (object)DBNull.Value;
            cmd.Parameters.Add("v_password", OracleDbType.Varchar2).Value = entity.Password ?? (object)DBNull.Value;
            cmd.Parameters.Add("v_personal_id", OracleDbType.Varchar2).Value = entity.Personal_Id ?? (object)DBNull.Value;
            cmd.Parameters.Add("v_role", OracleDbType.Int32).Value = entity.Role.HasValue ? (int)entity.Role.Value : (object)DBNull.Value;
            cmd.Parameters.Add("v_category", OracleDbType.Int32).Value = entity.Category.HasValue ? (int)entity.Category.Value : (object)DBNull.Value;
            cmd.Parameters.Add("v_rating", OracleDbType.Int32).Value = entity.Rating.HasValue ? entity.Rating.Value : (object)DBNull.Value;

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void UpdateEntity(int id, Doctor entity)
        {
            Doctor existingDoctor = GetEntity(id);

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
            cmd.CommandText = "olerning.PKG_IURI_DOCTORS.update_doctor";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("v_firstname", OracleDbType.Varchar2).Value = entity.FirstName ?? (object)DBNull.Value;
            cmd.Parameters.Add("v_lastname", OracleDbType.Varchar2).Value = entity.LastName ?? (object)DBNull.Value;
            cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = entity.Email ?? (object)DBNull.Value;
            cmd.Parameters.Add("v_password", OracleDbType.Varchar2).Value = entity.Password ?? (object)DBNull.Value;
            cmd.Parameters.Add("v_personal_id", OracleDbType.Varchar2).Value = entity.Personal_Id ?? (object)DBNull.Value;
            cmd.Parameters.Add("v_role", OracleDbType.Int32).Value = entity.Role.HasValue ? (int)entity.Role.Value : (object)DBNull.Value;
            cmd.Parameters.Add("v_category", OracleDbType.Int32).Value = entity.Category.HasValue ? (int)entity.Category.Value : (object)DBNull.Value;
            cmd.Parameters.Add("v_rating", OracleDbType.Int32).Value = entity.Rating.HasValue ? entity.Rating.Value : (object)DBNull.Value;

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void DeleteEntity(int id)
        {
            Doctor existingDoctor = GetEntity(id);

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_DOCTORS.delete_doctor";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = id;

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public Doctor GetEntity(int id, bool throwIfNotFound = true)
        {
            Doctor doctor = null;

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_DOCTORS.get_doctor_by_id";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            reader.Read();
            if (reader.HasRows)
            {
                doctor = new Doctor()
                {
                    Id = id,
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
                throw new UserNotFoundException($"Doctor with id:'{id}' does not exist");
            }

            conn.Close();

            return doctor;
        }

        public List<Doctor> GetEntities()
        {
            List<Doctor> doctors = new List<Doctor>();

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_DOCTORS.get_all_doctors";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Doctor doctor = new Doctor()
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

                doctors.Add(doctor);
            }

            conn.Close();
            return doctors;
        }

        public Doctor GetByEmail(string email)
        {
            return _accountRepository.GetDoctorByEmail(email, true);
        }

        public Doctor GetByPersonalId(string personalId)
        {
            return _accountRepository.GetDoctorByPersonalId(personalId, true);
        }

        public void UploadImage(int doctorId, IFormFile image)
        {
            Doctor existingDoctor = GetEntity(doctorId);

            if (image == null || image.Length == 0)
            {
                throw new FileUploadException("Please upload a valid file");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new FileUploadException("Invalid file type. Allowed types are: .jpg, .jpeg, .png, .gif");
            }

            byte[] photoData;
            using (var ms = new MemoryStream())
            {
                image.CopyTo(ms);
                photoData = ms.ToArray();
            }

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_DOCTORS.add_image";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = doctorId;
            cmd.Parameters.Add("v_image", OracleDbType.Blob).Value = photoData;

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public byte[] GetImage(int doctorId)
        {
            Doctor existingDoctor = GetEntity(doctorId);

            byte[] imageData = null;

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_DOCTORS.get_image_by_doctor_id";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = doctorId;
            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            reader.Read();
            if (reader.HasRows)
            {
                if (reader["image"] != DBNull.Value)
                {
                    imageData = (byte[])reader["image"];
                }
                else
                {
                    throw new FileDownloadException($"No image file was found for doctor with id: {doctorId}");
                }
            }

            return imageData;
        }

        public void UploadCV(int doctorId, IFormFile cv)
        {
            Doctor existingDoctor = GetEntity(doctorId);

            if (cv == null || cv.Length == 0)
            {
                throw new FileUploadException("Please upload a valid file");
            }

            var allowedExtensions = new[] {".pdf"};
            var fileExtension = Path.GetExtension(cv.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new FileUploadException("Invalid file type. Allowed types are: .pdf");
            }

            byte[] cvData;
            using (var ms = new MemoryStream())
            {
                cv.CopyTo(ms);
                cvData = ms.ToArray();
            }

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_DOCTORS.add_cv";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = doctorId;
            cmd.Parameters.Add("v_cv", OracleDbType.Blob).Value = cvData;

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public byte[] GetCV(int doctorId)
        {
            Doctor existingDoctor = GetEntity(doctorId);

            byte[] cvData = null;

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_DOCTORS.get_cv_by_doctor_id";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = doctorId;
            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            reader.Read();
            if (reader.HasRows)
            {
                if (reader["cv"] != DBNull.Value)
                {
                    cvData = (byte[])reader["cv"];
                }
                else
                {
                    throw new FileDownloadException($"No cv file was found for doctor with id: {doctorId}");
                }
            }

            return cvData;
        }

        public List<CategoryInfo> GetCategoryList()
        {
            List<CategoryInfo> categoryInfos = new List<CategoryInfo>();

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_DOCTORS.get_categories_count";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            foreach (int Id in Enum.GetValues(typeof(Category)))
            {
                categoryInfos.Add(new CategoryInfo() { Id = Id, Name = Enum.GetName(typeof(Category), Id), Count = 0 });
            }

            while (reader.Read())
            {
                categoryInfos.Find(x => x.Id == int.Parse(reader["category"].ToString())).Count = int.Parse(reader["category_count"].ToString());
            }

            conn.Close();

            return categoryInfos;
        }
    }
}
