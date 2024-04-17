using Azure.Data.Tables;
using MvcCoreSasStorage.Models;
using Newtonsoft.Json.Linq;
using System.Net;

namespace MvcCoreSasStorage.Services
{
    public class ServiceAzureAlumnos
    {
        //necesitamos la tabla alumnos
        private TableClient tablaAlumnos;
        //necesitamos la uri de acceso al token
        //que estará en configuration
        private string UrlApiToken;

        public ServiceAzureAlumnos(IConfiguration configuration)
        {
            this.UrlApiToken = configuration.GetValue<string>
                ("ApiUrls:ApiTokenSas");

        }

        //metodo para leer el token
        public async Task<string> GetTokenAsync(string curso)
        {

            using (WebClient client = new WebClient())
            {
                string request = "token/" + curso;

                client.Headers["content-type"] = "application/json";
                Uri uri =  new Uri(this.UrlApiToken + request);

                string data = await
                    client.DownloadStringTaskAsync(uri);

                JObject objJson = JObject.Parse(data);

                string token = objJson.GetValue("token").ToString();

                return token;
            }
        }

        //MÉTODO PARA RECUPERAR LOS ALUMNOS DESDE EL TOKEN
        public async Task<List<Alumno>>
            GetAlumnosAsync(string curso)
        {
            string token = await this.GetTokenAsync(curso);

            //creamos un uri con el token
            Uri uriToken = new Uri(token);

            //para acceder al recurso, creamos el recurso con su uri
            this.tablaAlumnos = new TableClient(uriToken);
            List<Alumno> alumnos = new List<Alumno>();

            //realizanos una consulta con filter para
            //recuperar todos los alumnos
            var query = this.tablaAlumnos.QueryAsync<Alumno>(filter: "");
            await foreach (var item in query)
            {
                alumnos.Add(item);
            }

            return alumnos;
        }
    }
}
