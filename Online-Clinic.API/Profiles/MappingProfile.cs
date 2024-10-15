using AutoMapper;
using Online_Clinic.API.DTOs;
using Online_Clinic.API.Models;

namespace Online_Clinic.API.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientResponse>();

            CreateMap<PatientRequest, Patient>();

            CreateMap<Doctor, DoctorResponse>();

            CreateMap<DoctorRequest, Doctor>();
        }
    }
}
