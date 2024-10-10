namespace Online_Clinic.API.Repositories.Oracle
{
    public class PKG_BASE
    {
        private IConfiguration _configuration;

        public PKG_BASE(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected string ConnStr
        {
            get { return this._configuration.GetConnectionString("OracleConnection"); }
        }
    }
}
