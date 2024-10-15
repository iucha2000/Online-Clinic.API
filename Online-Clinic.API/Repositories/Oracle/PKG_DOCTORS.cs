using Online_Clinic.API.Enums;
using Online_Clinic.API.Exceptions;
using Online_Clinic.API.Interfaces;
using Online_Clinic.API.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;
using Oracle.ManagedDataAccess.Types;

namespace Online_Clinic.API.Repositories.Oracle
{
    public class PKG_DOCTORS : PKG_BASE, IDoctorRepository
    {
        public PKG_DOCTORS(IConfiguration configuration) : base(configuration)
        {
        }

        public void AddEntity(Doctor entity)
        {
            Doctor existingDoctor = GetByEmail(entity.Email, throwIfNotFound: false);
            if (existingDoctor != null)
            {
                throw new UserAlreadyExistsException("Doctor with given email already exists");
            }

            existingDoctor = GetByPersonalId(entity.Personal_Id, throwIfNotFound: false);
            if (existingDoctor != null)
            {
                throw new UserAlreadyExistsException("Doctor with given personalId already exists");
            }

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_DOCTORS.add_doctor";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_firstname", OracleDbType.Varchar2).Value = entity.FirstName;
            cmd.Parameters.Add("v_lastname", OracleDbType.Varchar2).Value = entity.LastName;
            cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = entity.Email;
            cmd.Parameters.Add("v_password", OracleDbType.Varchar2).Value = entity.Password;
            cmd.Parameters.Add("v_personal_id", OracleDbType.Varchar2).Value = entity.Personal_Id;
            cmd.Parameters.Add("v_activationcode", OracleDbType.Int32).Value = entity.ActivationCode;
            cmd.Parameters.Add("v_role", OracleDbType.Int32).Value = entity.Role;
            cmd.Parameters.Add("v_category", OracleDbType.Int32).Value = entity.Category;
            cmd.Parameters.Add("v_rating", OracleDbType.Int32).Value = entity.Rating;
            cmd.Parameters.Add("v_image", OracleDbType.Blob).Value = entity.Image;
            cmd.Parameters.Add("v_cv", OracleDbType.Blob).Value = entity.CV;


            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void UpdateEntity(int id, Doctor entity)
        {
            Doctor existingDoctor = GetEntity(id);

            existingDoctor = GetByEmail(entity.Email, throwIfNotFound: false);
            if (existingDoctor != null)
            {
                throw new UserAlreadyExistsException("Doctor with given email already exists");
            }

            existingDoctor = GetByPersonalId(entity.Personal_Id, throwIfNotFound: false);
            if (existingDoctor != null)
            {
                throw new UserAlreadyExistsException("Doctor with given personalId already exists");
            }

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_DOCTORS.update_doctor";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("v_firstname", OracleDbType.Varchar2).Value = entity.FirstName;
            cmd.Parameters.Add("v_lastname", OracleDbType.Varchar2).Value = entity.LastName;
            cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = entity.Email;
            cmd.Parameters.Add("v_password", OracleDbType.Varchar2).Value = entity.Password;
            cmd.Parameters.Add("v_personal_id", OracleDbType.Varchar2).Value = entity.Personal_Id;
            cmd.Parameters.Add("v_activationcode", OracleDbType.Int32).Value = entity.ActivationCode;
            cmd.Parameters.Add("v_role", OracleDbType.Int32).Value = entity.Role;
            cmd.Parameters.Add("v_category", OracleDbType.Int32).Value = entity.Category;
            cmd.Parameters.Add("v_rating", OracleDbType.Int32).Value = entity.Rating;
            cmd.Parameters.Add("v_image", OracleDbType.Blob).Value = entity.Image;
            cmd.Parameters.Add("v_cv", OracleDbType.Blob).Value = entity.CV;


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
                    FirstName = reader["firstname"].ToString(),
                    LastName = reader["lastname"].ToString(),
                    Email = reader["email"].ToString(),
                    Password = reader["password"].ToString(),
                    Personal_Id = reader["personal_id"].ToString(),
                    ActivationCode = int.Parse(reader["activationcode"].ToString()),
                    Role = (Role)int.Parse(reader["role"].ToString()),
                    Category = (Category)int.Parse(reader["category"].ToString()),
                    Rating = int.Parse(reader["rating"].ToString())
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
                    FirstName = reader["firstname"].ToString(),
                    LastName = reader["lastname"].ToString(),
                    Email = reader["email"].ToString(),
                    Password = reader["password"].ToString(),
                    Personal_Id = reader["personal_id"].ToString(),
                    ActivationCode = int.Parse(reader["activationcode"].ToString()),
                    Role = (Role)int.Parse(reader["role"].ToString()),
                    Category = (Category)int.Parse(reader["category"].ToString()),
                    Rating = int.Parse(reader["rating"].ToString())
                };

                doctors.Add(doctor);
            }

            conn.Close();
            return doctors;
        }

        public Doctor GetByEmail(string email, bool throwIfNotFound = true)
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
                    FirstName = reader["firstname"].ToString(),
                    LastName = reader["lastname"].ToString(),
                    Email = reader["email"].ToString(),
                    Password = reader["password"].ToString(),
                    Personal_Id = reader["personal_id"].ToString(),
                    ActivationCode = int.Parse(reader["activationcode"].ToString()),
                    Role = (Role)int.Parse(reader["role"].ToString()),
                    Category = (Category)int.Parse(reader["category"].ToString()),
                    Rating = int.Parse(reader["rating"].ToString())
                };
            }
            else if (throwIfNotFound)
            {
                throw new UserNotFoundException($"Doctor with email:'{email}' does not exist");
            }

            conn.Close();

            return doctor;

        }

        public Doctor GetByPersonalId(string personalId, bool throwIfNotFound = true)
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
                    FirstName = reader["firstname"].ToString(),
                    LastName = reader["lastname"].ToString(),
                    Email = reader["email"].ToString(),
                    Password = reader["password"].ToString(),
                    Personal_Id = reader["personal_id"].ToString(),
                    ActivationCode = int.Parse(reader["activationcode"].ToString()),
                    Role = (Role)int.Parse(reader["role"].ToString()),
                    Category = (Category)int.Parse(reader["category"].ToString()),
                    Rating = int.Parse(reader["rating"].ToString())
                };
            }
            else if (throwIfNotFound)
            {
                throw new UserNotFoundException($"Doctor with personalId:'{personalId}' does not exist");
            }

            conn.Close();

            return doctor;
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

            var allowedExtensions = new[] {".txt", ".doc", ".docx", ".pdf"};
            var fileExtension = Path.GetExtension(cv.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new FileUploadException("Invalid file type. Allowed types are: .txt, .doc, .docx, .pdf");
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
    }
}
