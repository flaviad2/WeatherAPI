using Moq;
using Xunit;
using WebApplication2.WeatherRepository;
using WebApplication2.Controllers;
using Microsoft.AspNetCore.Mvc;
using Autofac.Extras.Moq;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WeatherForecastTests
{
    public class WeatherTests
    {
        
        public WeatherTests()
        {

        }
        private List<WeatherEntity> GetSampleWeathers()
        {

            WeatherEntity we1 = new WeatherEntity(1, DateTime.Now, DateTime.Now.TimeOfDay, -10, 10,
                                20, true, "alte info", SourceEnum.Sibiu);

            WeatherEntity we2 = new WeatherEntity(2, DateTime.Now, DateTime.Now.TimeOfDay, -20, 20,
                                15, true, "alte informatii utile", SourceEnum.Constanta);

            WeatherEntity we3 = new WeatherEntity(3, DateTime.Now, DateTime.Now.TimeOfDay, -30, 30,
                                10, true, "alte info", SourceEnum.Cluj);

            WeatherEntity we4 = new WeatherEntity(4, DateTime.Now, DateTime.Now.TimeOfDay, -5, 5,
                                60, true, "alte info", SourceEnum.Bucuresti);

            WeatherEntity we5 = new WeatherEntity(5, DateTime.Now, DateTime.Now.TimeOfDay, 0, 10,
                                40, true, "alte info", SourceEnum.Iasi);


            List<WeatherEntity> output = new List<WeatherEntity>();
            output.Add(we1);
            output.Add(we2);
            output.Add(we3);
            output.Add(we4);
            output.Add(we5);

            return output;
        }

       

        [Fact]
        public void GetAllTest()
        {
            var options = new DbContextOptionsBuilder<WeatherContext>()
                    .UseInMemoryDatabase(databaseName: "WeatherListDatabase1")
                    .Options;

            //insert seed cu o instanta de context 

            using (var context = new WeatherContext(options))
            {
                context.WeatherForecasts.Add(new WeatherEntity { Id = 1, Date = DateTime.Now, Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Cluj, MaximumTemperature = 30, MinimumTemperature = 20, OtherInformation = "alte info", PrecipitationsProbability = 20 });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 2, Date = DateTime.Now, Time = DateTime.Now.TimeOfDay, PrecipitationsProbability = 30, AtmosphericFenomens = false, DataSource = SourceEnum.Sibiu, MaximumTemperature = 30, MinimumTemperature = 29, OtherInformation = "alte info" });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 3, Date = DateTime.Now, Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Sibiu, MaximumTemperature = 29, MinimumTemperature = 28, OtherInformation = "altele", PrecipitationsProbability = 90 });
                context.SaveChanges();
            
            }

            //clean instance of context 
            using (var context = new WeatherContext(options))
            {
                WeatherRepository weatherRepository = new WeatherRepository(context);
                List<WeatherEntity> weatherEntities = weatherRepository.GetAll().Result;

                Assert.Equal(3, weatherEntities.Count);


                Assert.Equal(1, weatherEntities[0].Id);
                Assert.Equal(2, weatherEntities[1].Id);
                Assert.Equal(3 ,weatherEntities[2].Id);

                Assert.Equal(20, weatherEntities[0].MinimumTemperature);
                Assert.Equal(29, weatherEntities[1].MinimumTemperature);
                Assert.Equal(28, weatherEntities[2].MinimumTemperature);


                Assert.Equal(SourceEnum.Cluj, weatherEntities[0].DataSource);
                Assert.Equal(SourceEnum.Sibiu, weatherEntities[1].DataSource);
                Assert.Equal(SourceEnum.Sibiu, weatherEntities[2].DataSource);



                Assert.Equal(20, weatherEntities[0].PrecipitationsProbability);
                Assert.Equal(30 , weatherEntities[1].PrecipitationsProbability);
                Assert.Equal(90 , weatherEntities[2].PrecipitationsProbability);


                Assert.Equal(true, weatherEntities[0].AtmosphericFenomens);
                Assert.Equal(false, weatherEntities[1].AtmosphericFenomens);
                Assert.Equal(true, weatherEntities[2].AtmosphericFenomens); 

            }
        }


        [Fact]
        public void GetAllBetweenDatesTest()
        {
            var options = new DbContextOptionsBuilder<WeatherContext>()
                    .UseInMemoryDatabase(databaseName: "WeatherListDatabase2")
                    .Options;

            //insert seed cu o instanta de context 

            using (var context = new WeatherContext(options))
            {
                context.WeatherForecasts.Add(new WeatherEntity { Id = 1, Date = DateTime.Now.AddDays(10), Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Cluj, MaximumTemperature = 30, MinimumTemperature = 20, OtherInformation = "alte info", PrecipitationsProbability = 20 });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 2, Date = DateTime.Now.AddDays(20), Time = DateTime.Now.TimeOfDay, PrecipitationsProbability = 30, AtmosphericFenomens = false, DataSource = SourceEnum.Sibiu, MaximumTemperature = 30, MinimumTemperature = 29, OtherInformation = "alte info" });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 3, Date = DateTime.Now, Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Sibiu, MaximumTemperature = 29, MinimumTemperature = 28, OtherInformation = "altele", PrecipitationsProbability = 90 });
                context.SaveChanges();

            }

            //clean instance of context 
            using (var context = new WeatherContext(options))
            {
                WeatherRepository weatherRepository = new WeatherRepository(context);
                List<WeatherEntity> weatherEntities = weatherRepository.GetWeathersBetweenDates(DateTime.Now, DateTime.Now.AddDays(21)).Result;
                
                
                Assert.Equal(2, weatherEntities.Count);


                Assert.Equal(1, weatherEntities[0].Id);
                Assert.Equal(2, weatherEntities[1].Id);

                Assert.Equal(20, weatherEntities[0].MinimumTemperature);
                Assert.Equal(29, weatherEntities[1].MinimumTemperature);


                Assert.Equal(SourceEnum.Cluj, weatherEntities[0].DataSource);
                Assert.Equal(SourceEnum.Sibiu, weatherEntities[1].DataSource);

                Assert.Equal(20, weatherEntities[0].PrecipitationsProbability);
                Assert.Equal(30, weatherEntities[1].PrecipitationsProbability);

                Assert.Equal(true, weatherEntities[0].AtmosphericFenomens);
                Assert.Equal(false, weatherEntities[1].AtmosphericFenomens); 

            }
        }



        [Fact]
        public void GetAllFromDateTest()
        {
            var options = new DbContextOptionsBuilder<WeatherContext>()
                    .UseInMemoryDatabase(databaseName: "WeatherListDatabase3")
                    .Options;

            //insert seed cu o instanta de context 

            using (var context = new WeatherContext(options))
            {
                context.WeatherForecasts.Add(new WeatherEntity { Id = 1, Date = new DateTime(2022, 7, 10, 7, 10, 24), Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Cluj, MaximumTemperature = 30, MinimumTemperature = 20, OtherInformation = "alte info", PrecipitationsProbability = 20 });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 2, Date = new DateTime(2023, 7, 10, 7, 10, 24), Time = DateTime.Now.TimeOfDay, PrecipitationsProbability = 30, AtmosphericFenomens = false, DataSource = SourceEnum.Sibiu, MaximumTemperature = 30, MinimumTemperature = 29, OtherInformation = "alte info" });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 3, Date = new DateTime(2023, 7, 10, 7, 10, 24), Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Sibiu, MaximumTemperature = 29, MinimumTemperature = 28, OtherInformation = "altele", PrecipitationsProbability = 90 });
                context.SaveChanges();

            }

            //clean instance of context 
            using (var context = new WeatherContext(options))
            {
                WeatherRepository weatherRepository = new WeatherRepository(context);
                List<WeatherEntity> weatherEntities = weatherRepository.GetWeathersFromDay(new DateTime(2023, 7, 10, 7, 10, 24)).Result;
                
                
                Assert.Equal(2, weatherEntities.Count);

                Assert.Equal(2, weatherEntities[0].Id);
                Assert.Equal(3, weatherEntities[1].Id);

                Assert.Equal(29, weatherEntities[0].MinimumTemperature);
                Assert.Equal(28, weatherEntities[1].MinimumTemperature);


                Assert.Equal(SourceEnum.Sibiu, weatherEntities[0].DataSource);
                Assert.Equal(SourceEnum.Sibiu, weatherEntities[1].DataSource);


                Assert.Equal(30, weatherEntities[0].PrecipitationsProbability);
                Assert.Equal(90, weatherEntities[1].PrecipitationsProbability) ;


                Assert.Equal(false, weatherEntities[0].AtmosphericFenomens);
                Assert.Equal(true, weatherEntities[1].AtmosphericFenomens); 

            }
        }


        [Fact]
        public void GetAllNotTooFarTest()
        {
            var options = new DbContextOptionsBuilder<WeatherContext>()
                    .UseInMemoryDatabase(databaseName: "WeatherListDatabase4")
                    .Options;

            //insert seed cu o instanta de context 

            using (var context = new WeatherContext(options))
            {
                context.WeatherForecasts.Add(new WeatherEntity { Id = 1, Date = DateTime.Now.AddDays(10000), Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Cluj, MaximumTemperature = 30, MinimumTemperature = 20, OtherInformation = "alte info", PrecipitationsProbability = 20 });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 2, Date = DateTime.Now.AddDays(20), Time = DateTime.Now.TimeOfDay, PrecipitationsProbability = 30, AtmosphericFenomens = false, DataSource = SourceEnum.Sibiu, MaximumTemperature = 30, MinimumTemperature = 29, OtherInformation = "alte info" });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 3, Date = DateTime.Now, Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Sibiu, MaximumTemperature = 29, MinimumTemperature = 28, OtherInformation = "altele", PrecipitationsProbability = 90 });
                context.SaveChanges();

            }

            //clean instance of context 
            using (var context = new WeatherContext(options))
            {
                WeatherRepository weatherRepository = new WeatherRepository(context);
                List<WeatherEntity> weatherEntities = weatherRepository.GetWeathersNotTooFar(DateTime.Now).Result;
                
                
                Assert.Equal(2, weatherEntities.Count);

                Assert.Equal(2, weatherEntities[0].Id);
                Assert.Equal(3, weatherEntities[1].Id);

                Assert.Equal(29, weatherEntities[0].MinimumTemperature);
                Assert.Equal(28, weatherEntities[1].MinimumTemperature);


                Assert.Equal(SourceEnum.Sibiu, weatherEntities[0].DataSource);
                Assert.Equal(SourceEnum.Sibiu, weatherEntities[1].DataSource);



                Assert.Equal(30, weatherEntities[0].PrecipitationsProbability);
                Assert.Equal(90, weatherEntities[1].PrecipitationsProbability);

                Assert.Equal(false, weatherEntities[0].AtmosphericFenomens);
                Assert.Equal(true, weatherEntities[1].AtmosphericFenomens);


            }
        }


        [Fact]
        public async Task EditWeatherFromDayFromSource_ValidCall()
        {
            var options = new DbContextOptionsBuilder<WeatherContext>()
                    .UseInMemoryDatabase(databaseName: "WeatherListDatabase5")
                    .Options;

            //insert seed cu o instanta de context 

            using (var context = new WeatherContext(options))
            {
                context.WeatherForecasts.Add(new WeatherEntity { Id = 1, Date = new DateTime(2023, 7, 10, 7, 10, 24), Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Cluj, MaximumTemperature = 30, MinimumTemperature = 20, OtherInformation = "alte info", PrecipitationsProbability = 20 });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 2, Date = DateTime.Now.AddDays(20), Time = DateTime.Now.TimeOfDay, PrecipitationsProbability = 30, AtmosphericFenomens = false, DataSource = SourceEnum.Sibiu, MaximumTemperature = 30, MinimumTemperature = 29, OtherInformation = "alte info" });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 3, Date = DateTime.Now, Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Sibiu, MaximumTemperature = 29, MinimumTemperature = 28, OtherInformation = "altele", PrecipitationsProbability = 90 });
                context.SaveChanges();

            }

            //clean instance of context 
            using (var context = new WeatherContext(options))
            {
                WeatherRepository weatherRepository = new WeatherRepository(context);
                WeatherEntity newObj = new WeatherEntity { Id = 1, Date = new DateTime(2023, 7, 10, 7, 10, 24), Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Cluj, MaximumTemperature = -30, MinimumTemperature = -20, OtherInformation = "alte info", PrecipitationsProbability =100 };

                WeatherEntity weatherEntity = await weatherRepository.EditWeatherFromDayFromSource(new DateTime(2023, 7, 10, 7, 10, 24), SourceEnum.Cluj, newObj);

                
                Assert.Equal(1,weatherEntity.Id);
                Assert.Equal(new DateTime(2023, 7, 10, 7, 10, 24), weatherEntity.Date);
                Assert.Equal(SourceEnum.Cluj, weatherEntity.DataSource);
                Assert.Equal(-30, weatherEntity.MaximumTemperature);
                Assert.Equal(-20, weatherEntity.MinimumTemperature);
                Assert.Equal(100, weatherEntity.PrecipitationsProbability);
                
               
            }
        }



        [Fact]
        public async Task EditWeatherFromDayFromSource_InvalidCall()
        {
            var options = new DbContextOptionsBuilder<WeatherContext>()
                    .UseInMemoryDatabase(databaseName: "WeatherListDatabase6")
                    .Options;

            //insert seed cu o instanta de context 

            using (var context = new WeatherContext(options))
            {
                context.WeatherForecasts.Add(new WeatherEntity { Id = 1, Date = new DateTime(2022, 7, 10, 7, 10, 24), Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Cluj, MaximumTemperature = 30, MinimumTemperature = 20, OtherInformation = "alte info", PrecipitationsProbability = 20 });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 2, Date = DateTime.Now.AddDays(20), Time = DateTime.Now.TimeOfDay, PrecipitationsProbability = 30, AtmosphericFenomens = false, DataSource = SourceEnum.Sibiu, MaximumTemperature = 30, MinimumTemperature = 29, OtherInformation = "alte info" });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 3, Date = DateTime.Now, Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Sibiu, MaximumTemperature = 29, MinimumTemperature = 28, OtherInformation = "altele", PrecipitationsProbability = 90 });
                context.SaveChanges();

            }

            //clean instance of context 
            using (var context = new WeatherContext(options))
            {
                WeatherRepository weatherRepository = new WeatherRepository(context);
                WeatherEntity newObj = new WeatherEntity { Id = 1, Date = new DateTime(2023, 7, 10, 7, 10, 24), Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Cluj, MaximumTemperature = 30, MinimumTemperature = 20, OtherInformation = "alte info", PrecipitationsProbability = 20 };

                try
                {
                    WeatherEntity weatherEntity = await weatherRepository.EditWeatherFromDayFromSource(new DateTime(2021, 7, 10, 7, 10, 24), SourceEnum.Cluj, newObj);
                    Assert.False(true);
                } 
                catch(Exception e)
                {
                    Assert.True(true);

                }

                try
                {
                    WeatherEntity weatherEntity = await weatherRepository.EditWeatherFromDayFromSource(new DateTime(2022, 7, 10, 7, 10, 24), SourceEnum.Constanta, newObj);
                    Assert.False(true);
                }
                catch (Exception e)
                {
                    Assert.True(true);

                }
                try
                {
                    WeatherEntity weatherEntity = await weatherRepository.EditWeatherFromDayFromSource(new DateTime(2022, 12, 12, 7, 10, 24), SourceEnum.Bucuresti, newObj);
                    Assert.False(true); 
                }
                catch (Exception e)
                {
                    Assert.True(true);

                }

            }
        }


        [Fact]
        public async Task AddWeatherWithDate_ValidCall()
        {
            var options = new DbContextOptionsBuilder<WeatherContext>()
                    .UseInMemoryDatabase(databaseName: "WeatherListDatabase7")
                    .Options;

            //insert seed cu o instanta de context 

            using (var context = new WeatherContext(options))
            {
                context.WeatherForecasts.Add(new WeatherEntity { Id = 1, Date = new DateTime(2023, 7, 10, 7, 10, 24), Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Cluj, MaximumTemperature = 30, MinimumTemperature = 20, OtherInformation = "alte info", PrecipitationsProbability = 20 });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 2, Date = DateTime.Now.AddDays(20), Time = DateTime.Now.TimeOfDay, PrecipitationsProbability = 30, AtmosphericFenomens = false, DataSource = SourceEnum.Sibiu, MaximumTemperature = 30, MinimumTemperature = 29, OtherInformation = "alte info" });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 3, Date = DateTime.Now, Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Sibiu, MaximumTemperature = 29, MinimumTemperature = 28, OtherInformation = "altele", PrecipitationsProbability = 90 });
                context.SaveChanges();

            }

            //clean instance of context 
            using (var context = new WeatherContext(options))
            {
                WeatherRepository weatherRepository = new WeatherRepository(context);
                WeatherEntity newObj = new WeatherEntity { Id = 4, Date = new DateTime(2024, 06 , 10, 7, 10, 24), Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Cluj, MaximumTemperature = -30, MinimumTemperature = -20, OtherInformation = "alte info", PrecipitationsProbability = 100 };

                WeatherEntity weatherEntity = await weatherRepository.AddWeatherWithDate(new DateTime(2024, 06, 10, 7, 10, 24), newObj);

                Assert.Equal(4, weatherEntity.Id);
                Assert.Equal(new DateTime(2024, 06, 10, 7, 10, 24), weatherEntity.Date);
                Assert.Equal(SourceEnum.Cluj, weatherEntity.DataSource);
                Assert.Equal(-30, weatherEntity.MaximumTemperature);
                Assert.Equal(-20, weatherEntity.MinimumTemperature);
                Assert.Equal(100, weatherEntity.PrecipitationsProbability);

                List<WeatherEntity> weatherEntities = (List<WeatherEntity>)weatherRepository.GetAll().Result;

                Assert.Equal(4, weatherEntities.Count);



            }
        }



        [Fact]
        public async Task AddWeatherWithDate_InvalidCall()
        {
            var options = new DbContextOptionsBuilder<WeatherContext>()
                    .UseInMemoryDatabase(databaseName: "WeatherListDatabase8")
                    .Options;

            //insert seed cu o instanta de context 

            using (var context = new WeatherContext(options))
            {
                context.WeatherForecasts.Add(new WeatherEntity { Id = 1, Date = new DateTime(2022, 7, 10, 7, 10, 24), Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Cluj, MaximumTemperature = 30, MinimumTemperature = 20, OtherInformation = "alte info", PrecipitationsProbability = 20 });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 2, Date = DateTime.Now.AddDays(20), Time = DateTime.Now.TimeOfDay, PrecipitationsProbability = 30, AtmosphericFenomens = false, DataSource = SourceEnum.Sibiu, MaximumTemperature = 30, MinimumTemperature = 29, OtherInformation = "alte info" });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 3, Date = DateTime.Now, Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Sibiu, MaximumTemperature = 29, MinimumTemperature = 28, OtherInformation = "altele", PrecipitationsProbability = 90 });
                context.SaveChanges();

            }

            //clean instance of context 
            using (var context = new WeatherContext(options))
            {
                WeatherRepository weatherRepository = new WeatherRepository(context);
                WeatherEntity newObj = new WeatherEntity { Id = 4, Date = new DateTime(2022, 06, 10, 7, 10, 24), Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Cluj, MaximumTemperature = -30, MinimumTemperature = -20, OtherInformation = "alte info", PrecipitationsProbability = 100 };

                try
                {
                    WeatherEntity weatherEntity = await weatherRepository.AddWeatherWithDate(new DateTime(2022, 06, 10, 7, 10, 24), newObj);
                    Assert.True(false); 
                }
                catch(Exception e)
                {
                    Assert.True(true);
                }
               

                List<WeatherEntity> weatherEntities = weatherRepository.GetAll().Result;
                Assert.Equal(3, weatherEntities.Count);



            }
        }

        [Fact]
        public void DeleteWeathersFromDate_ValidCall()
        {
            var options = new DbContextOptionsBuilder<WeatherContext>()
                    .UseInMemoryDatabase(databaseName: "WeatherListDatabase9")
                    .Options;

            //insert seed cu o instanta de context 

            using (var context = new WeatherContext(options))
            {
                context.WeatherForecasts.Add(new WeatherEntity { Id = 1, Date = new DateTime(2024, 06, 10, 7, 10, 24), Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Cluj, MaximumTemperature = 30, MinimumTemperature = 20, OtherInformation = "alte info", PrecipitationsProbability = 20 });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 2, Date = new DateTime(2021, 06, 10, 7, 10, 24), Time = DateTime.Now.TimeOfDay, PrecipitationsProbability = 30, AtmosphericFenomens = false, DataSource = SourceEnum.Sibiu, MaximumTemperature = 30, MinimumTemperature = 29, OtherInformation = "alte info" });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 3, Date = new DateTime(2022, 06, 6, 7, 10, 24), Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Sibiu, MaximumTemperature = 29, MinimumTemperature = 28, OtherInformation = "altele", PrecipitationsProbability = 90 });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 4, Date = new DateTime(2022, 06, 6, 7, 10, 24), Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Sibiu, MaximumTemperature = 29, MinimumTemperature = 28, OtherInformation = "altele", PrecipitationsProbability = 90 });

                context.SaveChanges();

            }

            //clean instance of context 
            using (var context = new WeatherContext(options))
            {
                WeatherRepository weatherRepository = new WeatherRepository(context);
                List<WeatherEntity> weatherEntities = weatherRepository.DeleteWeathersFromDay(new DateTime(2022, 06, 06, 7, 10, 24)).Result;


                Assert.Equal(2, weatherEntities.Count);

                Assert.Equal(3, weatherEntities[0].Id);
                Assert.Equal(4, weatherEntities[1].Id);


                Assert.Equal(SourceEnum.Sibiu, weatherEntities[0].DataSource);
                Assert.Equal(SourceEnum.Sibiu, weatherEntities[1].DataSource);

                Assert.Equal(new DateTime(2022, 06, 06, 7, 10, 24), weatherEntities[0].Date);
                Assert.Equal(new DateTime(2022, 06, 06, 7, 10, 24), weatherEntities[1].Date); 





            }
        }

        [Fact]
        public void DeleteWeathersFromDate_NoEfectCall()
        {
            var options = new DbContextOptionsBuilder<WeatherContext>()
                    .UseInMemoryDatabase(databaseName: "WeatherListDatabase10")
                    .Options;

            //insert seed cu o instanta de context 

            using (var context = new WeatherContext(options))
            {
                context.WeatherForecasts.Add(new WeatherEntity { Id = 1, Date = new DateTime(2024, 06, 10, 7, 10, 24), Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Cluj, MaximumTemperature = 30, MinimumTemperature = 20, OtherInformation = "alte info", PrecipitationsProbability = 20 });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 2, Date = new DateTime(2021, 06, 10, 7, 10, 24), Time = DateTime.Now.TimeOfDay, PrecipitationsProbability = 30, AtmosphericFenomens = false, DataSource = SourceEnum.Sibiu, MaximumTemperature = 30, MinimumTemperature = 29, OtherInformation = "alte info" });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 3, Date = new DateTime(2022, 06, 6, 7, 10, 24), Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Sibiu, MaximumTemperature = 29, MinimumTemperature = 28, OtherInformation = "altele", PrecipitationsProbability = 90 });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 4, Date = new DateTime(2022, 06, 6, 7, 10, 24), Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Sibiu, MaximumTemperature = 29, MinimumTemperature = 28, OtherInformation = "altele", PrecipitationsProbability = 90 });

                context.SaveChanges();

            }

            //clean instance of context 
            using (var context = new WeatherContext(options))
            {
                WeatherRepository weatherRepository = new WeatherRepository(context);
                List<WeatherEntity> weatherEntities = weatherRepository.DeleteWeathersFromDay(new DateTime(2022, 06, 8, 7, 10, 24)).Result;


                Assert.Equal(0, weatherEntities.Count);
                //nu exista prognoza din acea zi


            }
        }


        [Fact]
        public void DeleteWeathersFromDate_InvalidCall()
        {
            var options = new DbContextOptionsBuilder<WeatherContext>()
                    .UseInMemoryDatabase(databaseName: "WeatherListDatabase11")
                    .Options;

            //insert seed cu o instanta de context 

            using (var context = new WeatherContext(options))
            {
                context.WeatherForecasts.Add(new WeatherEntity { Id = 1, Date = new DateTime(2024, 06, 10, 7, 10, 24), Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Cluj, MaximumTemperature = 30, MinimumTemperature = 20, OtherInformation = "alte info", PrecipitationsProbability = 20 });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 2, Date = new DateTime(2021, 06, 10, 7, 10, 24), Time = DateTime.Now.TimeOfDay, PrecipitationsProbability = 30, AtmosphericFenomens = false, DataSource = SourceEnum.Sibiu, MaximumTemperature = 30, MinimumTemperature = 29, OtherInformation = "alte info" });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 3, Date = new DateTime(2022, 06, 6, 7, 10, 24), Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Sibiu, MaximumTemperature = 29, MinimumTemperature = 28, OtherInformation = "altele", PrecipitationsProbability = 90 });
                context.WeatherForecasts.Add(new WeatherEntity { Id = 4, Date = new DateTime(2022, 06, 6, 7, 10, 24), Time = DateTime.Now.TimeOfDay, AtmosphericFenomens = true, DataSource = SourceEnum.Sibiu, MaximumTemperature = 29, MinimumTemperature = 28, OtherInformation = "altele", PrecipitationsProbability = 90 });

                context.SaveChanges();

            }

            //clean instance of context 
            using (var context = new WeatherContext(options))
            {
                WeatherRepository weatherRepository = new WeatherRepository(context);

                 try
                {
                    List<WeatherEntity> weatherEntities = weatherRepository.DeleteWeathersFromDay(new DateTime(2019, 06, 8, 7, 10, 24)).Result;

                    Assert.True(false);
                    //Asser.Fail ?
                }
                catch (Exception e)
                {
                    Assert.True(true);
                    //se arunca exceptie pt ca data e invalida
                }

                }
            }

    }


}


    
