﻿using BLLC.Extensions;
using BO.DTO.Requests;
using BO.DTO.Responses;
using BO.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Json;
using BO.DTO;

namespace BLLC.Services
{
    public class ClientService : IClientService
    {
        private readonly HttpClient _httpClient = AuthentificationService.Instance.httpClient;
        public ClientService() {}

        #region Client
        public async Task<PageResponse<Client>> GetAllClients(PageRequest pageRequest)
        {
            //  var reponse = await _httpClient.GetAsync($"books?page={pageRequest.Page}&pageSize={pageRequest.PageSize}");
            var reponse = await _httpClient.GetAsync($"clients{pageRequest.ToUriQuery()}");

            if (reponse.IsSuccessStatusCode)
            {
                using (var stream = await reponse.Content.ReadAsStreamAsync())
                {
                    PageResponse<Client> clientPage = await JsonSerializer.DeserializeAsync<PageResponse<Client>>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    return clientPage;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<ProfileDto> GetProfileByIdClient(int idClient)
        {
            //  var reponse = await _httpClient.GetAsync($"books?page={pageRequest.Page}&pageSize={pageRequest.PageSize}");
            var reponse = await _httpClient.GetAsync($"clients/{idClient}");

            if (reponse.IsSuccessStatusCode)
            {
                using (var stream = await reponse.Content.ReadAsStreamAsync())
                {
                    ProfileDto clientPage = await JsonSerializer.DeserializeAsync<ProfileDto>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    return clientPage;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<Client> CreateClient(Client client)
        {
            CreateClientRequest createClientRequest = new CreateClientRequest()
            {
                Nom = client.Nom,
                Prenom = client.Prenom,
                Email = client.Email,
                Tel = client.Tel,
                Login = client.Login,
                Password= client.Password
            };

            var reponse = await _httpClient.PostAsync("clients",
                new StringContent(
                    JsonSerializer.Serialize(createClientRequest), Encoding.UTF8, "application/json"
                    )
                );

            if (reponse.IsSuccessStatusCode)
            {
                //using (var stream = await reponse.Content.ReadAsStreamAsync())
                //{
                //    Client clientNew = await JsonSerializer.DeserializeAsync<Client>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                //    return clientNew;
                //}

                Client clientNew = await reponse.Content.ReadFromJsonAsync<Client>();
                return clientNew;
            }
            else
            {
                var r = await reponse.Content.ReadAsStringAsync();
                return null;
            }
        }


        public async Task<Client> PutClient(Client client)


        {
            var reponse = await _httpClient.PutAsync($"clients/" + client.Id
                ,
                new StringContent(
                    JsonSerializer.Serialize(client), Encoding.UTF8, "application/json"
                    )
                );

            if (reponse.IsSuccessStatusCode)
            {
                using (var stream = await reponse.Content.ReadAsStreamAsync())
                {
                    Client clientModified = await JsonSerializer.DeserializeAsync<Client>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    return clientModified;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteClient(Client client)
        {
            if (client?.Id != null)
            {
                try
                {
                    await _httpClient.DeleteAsync($"clients/{ client.Id}");
                    return true;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("An error occurred" + e.Message);
                    return false;
                }
            }
            return false;

        }


        #endregion
        #region Reservations
        public async Task<Reservation> CreateReservations(Reservation reservation)
        {
             var reponse = await _httpClient.PostAsync("reservations",
                new StringContent(
                    JsonSerializer.Serialize(reservation), Encoding.UTF8, "application/json"
                    )
                );

            if (reponse.IsSuccessStatusCode)
            {
                using (var stream = await reponse.Content.ReadAsStreamAsync())
                {
                    Reservation reservationtNew = await JsonSerializer.DeserializeAsync<Reservation>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    return reservationtNew;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<PageResponse<Reservation>> GetAllReservations(PageRequest pageRequest)
        {
            //  var reponse = await _httpClient.GetAsync($"books?page={pageRequest.Page}&pageSize={pageRequest.PageSize}");
            var reponse = await _httpClient.GetAsync($"reservations{pageRequest.ToUriQuery()}");

            if (reponse.IsSuccessStatusCode)
            {
                using (var stream = await reponse.Content.ReadAsStreamAsync())
                {
                    PageResponse<Reservation> reservationPage = await JsonSerializer.DeserializeAsync<PageResponse<Reservation>>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    return reservationPage;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<Reservation> GetDetailsResa(int idResa)
        {
            var reponse = await _httpClient.GetAsync($"reservations/{idResa}");

            if (reponse.IsSuccessStatusCode)
            {
                using (var stream = await reponse.Content.ReadAsStreamAsync())
                {
                    Reservation reservation = await JsonSerializer.DeserializeAsync<Reservation>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    return reservation;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<Reservation> PutReservations(Reservation reservation)
        {
            var reponse = await _httpClient.PutAsync($"reservations/" + reservation.Id
                ,
                new StringContent(
                    JsonSerializer.Serialize(reservation), Encoding.UTF8, "application/json"
                    )
                );

            if (reponse.IsSuccessStatusCode)
            {
                using (var stream = await reponse.Content.ReadAsStreamAsync())
                {
                    Reservation reservationModified = await JsonSerializer.DeserializeAsync<Reservation>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    return reservationModified;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteReservations(Reservation reservation)
        {
            if (reservation?.Id != null)
            {
                try
                {
                    await _httpClient.DeleteAsync($"reservations/{reservation.Id}");
                    return true;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("An error occurred> " + e.Message);
                    return false;
                }
            }
            return false;
        }
        #endregion
    }
}
