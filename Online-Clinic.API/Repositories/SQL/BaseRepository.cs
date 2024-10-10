namespace Online_Clinic.API.Repositories.SQL
{
    public class BaseRepository
    {
        private IConfiguration _configuration;
        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected string ConnStr
        {
            get { return this._configuration.GetConnectionString("SqlConnection"); }
        }
    }
}
