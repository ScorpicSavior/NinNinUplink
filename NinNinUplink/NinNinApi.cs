/*
 * Copyright (C) 2016 ScorpicSavior
 * 
 * This file is part of NinNinUplink.
 * 
 * NinNinUplink is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * NinNinUplink is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with NinNinUplink.  If not, see <http://www.gnu.org/licenses/>.
 */

using CS;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NinNinUplink
{
    class NinNinApi : IDisposable
    {
        private HttpClient m_httpClient;
        private string m_apiKey;

        public NinNinApi(string baseAddress, string apiKey)
        {
            this.m_httpClient = new HttpClient();
            this.m_httpClient.BaseAddress = new Uri(baseAddress);
            this.m_apiKey = apiKey;
        }

        public void Dispose()
        {
            this.m_httpClient.Dispose();
        }

        private Task<HttpResponseMessage> ApiPost(string path, MPackMap payload)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, "/api/v1/" + path);
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.m_apiKey);
            var content = new ByteArrayContent(payload.EncodeToBytes());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-msgpack");
            req.Content = content;
            return this.m_httpClient.SendAsync(req);
        }

        public Task<HttpResponseMessage> CheckApiKey()
        {
            var req = new HttpRequestMessage(HttpMethod.Options, "/api/v1/");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.m_apiKey);
            return this.m_httpClient.SendAsync(req);
        }

        public Task<HttpResponseMessage> PostPopulations(string timestamp, string serverTime, string mapName, int[] populations)
        {
            MPackMap payload = new MPackMap {
                    {"ts", timestamp},
                    {"server_time", serverTime},
                    {"populations", MPack.From(populations)}
                };
            return this.ApiPost("populations/" + mapName, payload);
        }
    }
}
