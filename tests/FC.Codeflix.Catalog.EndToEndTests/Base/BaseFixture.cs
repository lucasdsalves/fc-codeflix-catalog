using FC.Codeflix.Catalog.Infra.Data.EF;

namespace FC.Codeflix.Catalog.EndToEndTests.Base
{
    public abstract class BaseFixture
    {
        public Faker Faker { get; set; }

        public ApiClient ApiClient { get; set; }
        public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }
        public HttpClient HttpClient { get; set; }

        protected BaseFixture()
        {
            Faker = new Faker("pt_BR");
            WebAppFactory = new CustomWebApplicationFactory<Program>();
            HttpClient = WebAppFactory.CreateClient();
            ApiClient = new ApiClient(HttpClient);
        }

        public CodeflixCatalogDbContext CreateDbContext()
        {
            var context = new CodeflixCatalogDbContext(
                                new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
                                    .UseInMemoryDatabase("end2end-tests-db")
                                    .Options
                                );

            return context;
        }
    }
}
