﻿using System.Collections.Generic;
using System.Net.Http;
using Elsa.Contracts;
using Elsa.Expressions;
using Elsa.Models;
using Elsa.Runtime.Contracts;

namespace Elsa.Activities.Http
{
    public class HttpEndpoint : CodeActivity, ITrigger
    {
        public IExpression<string> Path { get; set; } = default!;
        public IExpression<ICollection<string>> SupportedMethods { get; set; } = new Literal<ICollection<string>>(new[] { HttpMethod.Get.Method });
    }
}