using Online_Clinic.API.Enums;
using Online_Clinic.API.Exceptions;
using Online_Clinic.API.Interfaces;
using Online_Clinic.API.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Numerics;

namespace Online_Clinic.API.Repositories.Oracle
{
    public class PKG_RESERVATIONS : PKG_BASE, IReservationRepository
    {

        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;

        public PKG_RESERVATIONS(IConfiguration configuration, IPatientRepository patientRepository, IDoctorRepository doctorRepository) : base(configuration)
        {
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
        }

        public void AddEntity(Reservation entity)
        {
            Patient patient = _patientRepository.GetEntity(entity.PatientId);
            Doctor doctor = _doctorRepository.GetEntity(entity.DoctorId);

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_RESERVATIONS.add_reservation";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_patientid", OracleDbType.Int32).Value = patient.Id;
            cmd.Parameters.Add("v_doctorid", OracleDbType.Int32).Value = doctor.Id;
            cmd.Parameters.Add("v_description", OracleDbType.Varchar2).Value = entity.Description ?? (object)DBNull.Value;
            cmd.Parameters.Add("v_reservation_date", OracleDbType.Date).Value = entity.ReservationDate ?? (object)DBNull.Value;

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void UpdateEntity(int id, Reservation entity)
        {
            Reservation existingReservation = GetEntity(id);
            Patient patient = _patientRepository.GetEntity(entity.PatientId);
            Doctor doctor = _doctorRepository.GetEntity (entity.DoctorId);

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_RESERVATIONS.update_reservation";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("v_patientid", OracleDbType.Int32).Value = patient.Id;
            cmd.Parameters.Add("v_doctorid", OracleDbType.Int32).Value = doctor.Id;
            cmd.Parameters.Add("v_description", OracleDbType.Varchar2).Value = entity.Description ?? (object)DBNull.Value;
            cmd.Parameters.Add("v_reservation_date", OracleDbType.Date).Value = entity.ReservationDate ?? (object)DBNull.Value;

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void DeleteEntity(int id)
        {
            Reservation existingReservation = GetEntity(id);

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_RESERVATIONS.delete_reservation";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = id;

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public Reservation GetEntity(int id, bool throwIfNotFound = true)
        {
            Reservation reservation = null;

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_RESERVATIONS.get_reservation_by_id";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_id", OracleDbType.Int32).Value = id;
            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            reader.Read();
            if (reader.HasRows)
            {
                reservation = new Reservation()
                {
                    Id = id,
                    PatientId = int.Parse(reader["patientid"].ToString()),
                    DoctorId = int.Parse(reader["doctorid"].ToString()),
                    Description = reader["description"] != DBNull.Value ? reader["description"].ToString() : null,
                    ReservationDate = reader["reservation_date"] != DBNull.Value ? Convert.ToDateTime(reader["reservation_date"]) : null
                };
            }
            else if (throwIfNotFound)
            {
                throw new UserNotFoundException($"Reservation with id:'{id}' does not exist");
            }

            conn.Close();

            return reservation;
        }

        public List<Reservation> GetEntities()
        {
            List<Reservation> reservations = new List<Reservation>();

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_RESERVATIONS.get_all_reservations";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Reservation reservation = new Reservation()
                {
                    Id = int.Parse(reader["id"].ToString()),
                    PatientId = int.Parse(reader["patientid"].ToString()),
                    DoctorId = int.Parse(reader["doctorid"].ToString()),
                    Description = reader["description"] != DBNull.Value ? reader["description"].ToString() : null,
                    ReservationDate = reader["reservation_date"] != DBNull.Value ? Convert.ToDateTime(reader["reservation_date"]) : null
                };

                reservations.Add(reservation);
            }

            conn.Close();
            return reservations;
        }

        public List<Reservation> GetByPatientId(int patientId)
        {
            Patient patient = _patientRepository.GetEntity(patientId);

            List<Reservation> reservations = new List<Reservation>();

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_RESERVATIONS.get_reservations_by_patient_id";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_patient_id", OracleDbType.Int32).Value = patient.Id;
            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Reservation reservation = new Reservation()
                {
                    Id = int.Parse(reader["id"].ToString()),
                    PatientId = int.Parse(reader["patientid"].ToString()),
                    DoctorId = int.Parse(reader["doctorid"].ToString()),
                    Description = reader["description"] != DBNull.Value ? reader["description"].ToString() : null,
                    ReservationDate = reader["reservation_date"] != DBNull.Value ? Convert.ToDateTime(reader["reservation_date"]) : null
                };

                reservations.Add(reservation);
            }

            conn.Close();
            return reservations;
        }

        public List<Reservation> GetByDoctorId(int doctorId)
        {
            Doctor doctor = _doctorRepository.GetEntity(doctorId);

            List<Reservation> reservations = new List<Reservation>();

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_IURI_RESERVATIONS.get_reservations_by_doctor_id";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_doctor_id", OracleDbType.Int32).Value = doctor.Id;
            cmd.Parameters.Add("v_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Reservation reservation = new Reservation()
                {
                    Id = int.Parse(reader["id"].ToString()),
                    PatientId = int.Parse(reader["patientid"].ToString()),
                    DoctorId = int.Parse(reader["doctorid"].ToString()),
                    Description = reader["description"] != DBNull.Value ? reader["description"].ToString() : null,
                    ReservationDate = reader["reservation_date"] != DBNull.Value ? Convert.ToDateTime(reader["reservation_date"]) : null
                };

                reservations.Add(reservation);
            }

            conn.Close();
            return reservations;
        }
    }
}
