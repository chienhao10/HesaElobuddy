// Decompiled with JetBrains decompiler
// Type: BananaLib.RestService.LoginQueue
// Assembly: BananaLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75213AF3-E339-4AEB-B3FE-095F85BC5F53
// Assembly location: C:\Users\Hesa\Desktop\eZ\BananaLib.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BananaLib.RestService
{
    public class LoginQueue
    {
        private readonly LoLHttpClient _httpClient = new LoLHttpClient();
        public const string ACCOUNT_BANNED = "account_banned";
        public const string ACCOUNT_TRANSFERRED = "account_transferred";
        public const string INVALID_CREDENTIALS = "invalid_credentials";
        public const string SERVER_FULL = "server_full";
        public const string ACCOUNT_INACTIVE = "account_inactive";
        public const string ATTEMPT_RATE_TOO_FAST = "attempt_rate_too_fast";
        public const string FAILED_STATUS = "FAILED";
        public const string BUSY_STATUS = "BUSY";
        public const string LOGIN_STATUS = "LOGIN";
        public const string QUEUE_STATUS = "QUEUE";
        private string _authUrl;
        private string _tickerUrl;
        private string _tokenUrl;
        private bool _useGarena;
        private Region _region;
        private int _timeoutSeconds;
        private int _authRetries;

        public string Username { get; set; }

        public string Password { get; set; }

        public string User { get; set; }
        public long UserId { get; set; }
        public string Token { get; set; }

        public Region Region
        {
            get
            {
                return this._region;
            }
            set
            {
                this._region = value;
                string str = value.LoginQueueServer() + "login-queue/rest/queues/lol/";
                this._authUrl = str + "authenticate/";
                this._tickerUrl = str + "ticker/";
                this._tokenUrl = str + "token/";
                this._useGarena = value.UseGarena();
            }
        }

        public int TimeoutSeconds
        {
            get
            {
                return this._timeoutSeconds;
            }
            set
            {
                this._timeoutSeconds = value < 20 ? 60 : value;
            }
        }

        public int AuthRetries
        {
            get
            {
                return this._authRetries;
            }
            set
            {
                this._authRetries = value < 1 ? 5 : value;
            }
        }

        public AuthResult AuthResult { get; private set; }

        public string AuthToken { get; private set; }

        public event LoginQueue.OnAuthFailedHandler OnAuthFailed;

        public event LoginQueue.OnLoginQueueUpdateHandler OnLoginQueueUpdate;

        public event LoginQueue.StatusMessageUpdateHandler OnUpdateStatusMessage;

        public LoginQueue(int timeoutSeconds = 60, int retries = 5)
        {
            this.TimeoutSeconds = timeoutSeconds;
            this.AuthRetries = retries;
        }

        public LoginQueue(string username, string password, Region region)
          : this(60, 5)
        {
            this.SetUserCredentials(username, password, region);
        }

        public void SetUserCredentials(string username, string password, Region region)
        {
            this.Username = username;
            this.Password = password;
            this.Region = region;
        }

        private string reToken(string s)
        {
            string s1 = s.Replace("/", "%2F");
            s1 = s1.Replace("+", "%2B");
            s1 = s1.Replace("=", "%3D");
            return s1;
        }

        public async Task<AuthResult> GetAuthResult()
        {
            string payload = WebUtility.UrlEncode(string.Format("user={0},password={1}", this.Username, this.Password));
            string query = "payload=" + payload;
            if (_useGarena)
            {
                payload = reToken(this.Password);
                query = "payload=8393%20" + payload;
            }
            string str = await (await this._httpClient.PostAsync(this._authUrl, query, false).ConfigureAwait(false)).Content.ReadAsStringAsync().ConfigureAwait(false);

            if (str.Contains("Access denied") && str.Contains("banned your IP"))
                throw new IpBannedException();
            return JsonConvert.DeserializeObject<AuthResult>(str);
        }

        public async Task<bool> GetAuthToken()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int retriesRemaining = this.AuthRetries;
            try
            {
                while (sw.ElapsedMilliseconds / 1000L <= (long)this.TimeoutSeconds)
                {
                    int num = 0;
                    try
                    {
                        this.AuthResult = await this.GetAuthResult().ConfigureAwait(false);
                        this.User = this.AuthResult.User;
                        this.Token = this.AuthResult.IdToken;
                        //this.UserId = this.AuthResult.Lqt.AccountId;
                    }
                    catch (IpBannedException ex)
                    {
                        // ISSUE: reference to a compiler-generated field
                        LoginQueue.OnAuthFailedHandler onAuthFailed = this.OnAuthFailed;
                        if (onAuthFailed != null)
                        {
                            string message = "Your IP has been banned due to too many requests.";
                            onAuthFailed(message);
                        }
                        return false;
                    }
                    catch (JsonReaderException ex)
                    {
                        num = 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                    }
                    if (num == 1)
                        await Task.Delay(1000).ConfigureAwait(false);
                    else if (this.AuthResult.Reason == null)
                    {
                        if (retriesRemaining > 0)
                        {
                            --retriesRemaining;
                        }
                        else
                        {
                            // ISSUE: reference to a compiler-generated field
                            LoginQueue.OnAuthFailedHandler onAuthFailed = this.OnAuthFailed;
                            if (onAuthFailed != null)
                            {
                                string message = "Unable to get Auth Token.";
                                onAuthFailed(message);
                            }
                            return false;
                        }
                    }
                    else
                    {
                        string reason = this.AuthResult.Reason;
                        if (!(reason == "attempt_rate_too_fast"))
                        {
                            if (!(reason == "invalid_credentials"))
                            {
                                if (!(reason == "account_banned"))
                                {
                                    if (!(reason == "account_transferred"))
                                    {
                                        if (!(reason == "account_inactive"))
                                        {
                                            if (reason == "server_full")
                                            {
                                                // ISSUE: reference to a compiler-generated field
                                                LoginQueue.OnAuthFailedHandler onAuthFailed = this.OnAuthFailed;
                                                if (onAuthFailed != null)
                                                {
                                                    string message = "Server is full. Try again later.";
                                                    onAuthFailed(message);
                                                }
                                                return false;
                                            }
                                            if (this.AuthResult.Status == "QUEUE" && this.AuthResult.Tickers != null)
                                            {
                                                await this.WaitInQueue().ConfigureAwait(false);
                                                this.AuthResult.Lqt.Resources = (string)null;
                                                this.AuthResult.Lqt.Other = (string)null;
                                                HttpResponseMessage httpResponseMessage = await this._httpClient.PostAsync(this._tokenUrl, this.AuthResult.Lqt.ToString(), true).ConfigureAwait(false);
                                                if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                                                {
                                                    this.AuthToken = JsonConvert.DeserializeObject<AuthResult>(await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false)).Lqt.ToString();
                                                    return true;
                                                }
                                            }
                                            if (this.AuthResult.Status == "LOGIN")
                                            {
                                                this.AuthToken = this.AuthResult.Lqt.ToString();
                                                return true;
                                            }
                                            await Task.Delay(1000).ConfigureAwait(false);
                                        }
                                        else
                                        {
                                            // ISSUE: reference to a compiler-generated field
                                            LoginQueue.OnAuthFailedHandler onAuthFailed = this.OnAuthFailed;
                                            if (onAuthFailed != null)
                                            {
                                                string message = "Account currently inactive.";
                                                onAuthFailed(message);
                                            }
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        // ISSUE: reference to a compiler-generated field
                                        LoginQueue.OnAuthFailedHandler onAuthFailed = this.OnAuthFailed;
                                        if (onAuthFailed != null)
                                        {
                                            string message = string.Format("Account transferred {0}", (object)this.AuthResult.Destination).Trim();
                                            onAuthFailed(message);
                                        }
                                        return false;
                                    }
                                }
                                else
                                {
                                    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(this.AuthResult.Banned);
                                    // ISSUE: reference to a compiler-generated field
                                    LoginQueue.OnAuthFailedHandler onAuthFailed = this.OnAuthFailed;
                                    if (onAuthFailed != null)
                                    {
                                        string message = string.Format("Account banned {0}", (object)dateTime.ToString("d", (IFormatProvider)CultureInfo.CurrentCulture));
                                        onAuthFailed(message);
                                    }
                                    return false;
                                }
                            }
                            else
                            {
                                // ISSUE: reference to a compiler-generated field
                                LoginQueue.OnAuthFailedHandler onAuthFailed = this.OnAuthFailed;
                                if (onAuthFailed != null)
                                {
                                    string message = "Incorrect username or password.";
                                    onAuthFailed(message);
                                }
                                return false;
                            }
                        }
                        else
                        {
                            // ISSUE: reference to a compiler-generated field
                            LoginQueue.StatusMessageUpdateHandler updateStatusMessage = this.OnUpdateStatusMessage;
                            if (updateStatusMessage != null)
                            {
                                string e = string.Format("Login rate too fast. Waiting {0} s.", (object)this.AuthResult.RetryWait);
                                updateStatusMessage((object)this, e);
                            }
                            await Task.Delay(TimeSpan.FromSeconds((double)this.AuthResult.RetryWait)).ConfigureAwait(false);
                            sw.Restart();
                        }
                    }
                }
                sw.Stop();
                throw new TimeoutException();
            }
            catch (TimeoutException ex)
            {
                // ISSUE: reference to a compiler-generated field
                LoginQueue.OnAuthFailedHandler onAuthFailed = this.OnAuthFailed;
                if (onAuthFailed != null)
                {
                    string message = "Timeout: queue time too long.";
                    onAuthFailed(message);
                }
            }
            finally
            {
                sw.Stop();
            }
            return false;
        }

        private async Task WaitInQueue()
        {
            int id = -1;
            int current = -1;
            using (IEnumerator<Ticker> enumerator = this.AuthResult.Tickers.Where<Ticker>((Func<Ticker, bool>)(ticker => ticker.Node == this.AuthResult.Node)).GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    Ticker current1 = enumerator.Current;
                    id = current1.Id;
                    current = current1.Current;
                }
            }
            int cycleNum = 1;
            double averageMillisPerCycle = 0.0;
            while (id - current > 0)
            {
                DateTime cycleStartTime = DateTime.Now;
                HttpResponseMessage httpResponseMessage = await this._httpClient.GetAsync(this._tickerUrl + this.AuthResult.Champ).ConfigureAwait(false);
                if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    try
                    {
                        current = int.Parse((string)JsonConvert.DeserializeObject<Dictionary<string, object>>(await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false))[this.AuthResult.Node.ToString()], NumberStyles.HexNumber);
                        int num = Math.Max(0, id - current);
                        if (num == 0)
                            break;
                        // ISSUE: reference to a compiler-generated field
                        LoginQueue.OnLoginQueueUpdateHandler loginQueueUpdate = this.OnLoginQueueUpdate;
                        if (loginQueueUpdate != null)
                        {
                            int positionInLine = num;
                            loginQueueUpdate(positionInLine);
                        }
                        // ISSUE: reference to a compiler-generated field
                        LoginQueue.StatusMessageUpdateHandler updateStatusMessage = this.OnUpdateStatusMessage;
                        if (updateStatusMessage != null)
                        {
                            string e = string.Format("In login queue at position {0}", (object)num);
                            updateStatusMessage((object)this, e);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    await Task.Delay(this.AuthResult.Delay).ConfigureAwait(false);
                }
                double totalMilliseconds = (DateTime.Now - cycleStartTime).TotalMilliseconds;
                averageMillisPerCycle = averageMillisPerCycle > 0.0 ? (averageMillisPerCycle + totalMilliseconds) / 2.0 : totalMilliseconds;
                if (cycleNum == 2)
                {
                    double num = averageMillisPerCycle / (double)(1 / this.AuthResult.Rate * 60);
                    if ((int)(averageMillisPerCycle * (num / (1.0 - num)) * ((1.0 + Math.Pow(1.333, 2.0)) / 2.0) / 1000.0) > this.TimeoutSeconds)
                        throw new TimeoutException();
                }
                ++cycleNum;
            }
        }

        public delegate void OnAuthFailedHandler(string message);

        public delegate void OnLoginQueueUpdateHandler(int positionInLine);

        public delegate void StatusMessageUpdateHandler(object sender, string e);
    }
}