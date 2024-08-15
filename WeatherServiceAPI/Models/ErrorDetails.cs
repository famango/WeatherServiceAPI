// <copyright file="ErrorDetails.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WeatherServiceAPI.Models
{
    using System.Text.Json;

    /// <summary>
    /// Class ErrorDetails model.
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// Gets or sets StatusCode.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets Message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Method for converting <see cref="ErrorDetails"/> to string.
        /// </summary>
        /// <returns>Returns a serialize representation of the <see cref="ErrorDetails"/> model.</returns>
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
