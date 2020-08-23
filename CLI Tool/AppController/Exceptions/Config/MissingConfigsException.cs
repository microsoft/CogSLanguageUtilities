﻿using CustomTextCliUtils.Configs.Consts;

namespace CustomTextCliUtils.AppController.Exceptions.Config
{
    class MissingConfigsException : CliException
    {
        public MissingConfigsException()
            : base(ConstructMessage())
        { }

        public static string ConstructMessage()
        {
            return $"Please add the required configs using the command\n{Constants.ToolName} config set";
        }
    }
}