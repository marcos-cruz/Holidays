using System.Net;

namespace Bigai.Holidays.Shared.Domain.Commands
{
    /// <summary>
    /// <see cref="CommandResult"/> represents the result of executing a command or action.
    /// </summary>
    public class CommandResult
    {
        #region Properties

        /// <summary>
        /// Determines whether the execution of a command or action has completed successfully.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Message returned by executing a command or action.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Response code, according to http status code.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Elapsed time measured by the current instance, in milliseconds.
        /// </summary>
        public long ElapsedTime { get; set; }

        /// <summary>
        /// Data produced by executing the command or action.
        /// </summary>
        public object Data { get; set; }

        #endregion

        #region Constructor

        private CommandResult(bool success, HttpStatusCode statusCode, string message)
        {
            Success = success;
            StatusCode = (int)statusCode;
            Message = message.Replace("  "," ").Replace(" .", ".");
            ElapsedTime = 0;
            Data = null;
        }

        #endregion

        #region Public Methdos

        /// <summary>
        /// Execution occurred successfully, status code 200.
        /// </summary>
        /// <param name="message">Message to the requesting interface.</param>
        /// <returns>Execution occurred successfully, status code 200.</returns>
        public static CommandResult Ok(string message)
        {
            return new CommandResult(true, HttpStatusCode.OK, message);
        }

        /// <summary>
        /// Execution occurred successfully, status code 201.
        /// </summary>
        /// <param name="message">Message to the requesting interface.</param>
        /// <returns>Execution occurred successfully, status code 201.</returns>
        public static CommandResult Created(string message)
        {
            return new CommandResult(true, HttpStatusCode.Created, message);
        }

        /// <summary>
        /// An error occurred while executing the command, status code 400.
        /// </summary>
        /// <param name="message">Message to the requesting interface.</param>
        /// <returns>Occurred while executing the command, status code 400.</returns>
        public static CommandResult BadRequest(string message)
        {
            return new CommandResult(false, HttpStatusCode.BadRequest, message);
        }

        /// <summary>
        /// An error occurred while executing the command, status code 500.
        /// </summary>
        /// <param name="message">Message to the requesting interface.</param>
        /// <returns>Occurred while executing the command, status code 500.</returns>
        public static CommandResult InternalServerError(string message)
        {
            return new CommandResult(false, HttpStatusCode.InternalServerError, message);
        }

        /// <summary>
        /// An error occurred while executing the command, status code 503.
        /// </summary>
        /// <param name="message">Message to the requesting interface.</param>
        /// <returns>Occurred while executing the command, status code 503.</returns>
        public static CommandResult ServiceUnavailable(string message)
        {
            return new CommandResult(false, HttpStatusCode.ServiceUnavailable, message);
        }

        #endregion
    }
}
