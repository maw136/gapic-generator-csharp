﻿// Copyright 2019 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// Generated code. DO NOT EDIT!

#pragma warning disable CS8981

namespace Testing.Snippets.Snippets
{
    // [START snippets_generated_Snippets_MethodDefaultValues_async_flattened]
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ts = Testing.Snippets;

    public sealed partial class GeneratedSnippetsClientSnippets
    {
        /// <summary>Snippet for MethodDefaultValuesAsync</summary>
        /// <remarks>
        /// This snippet has been automatically generated and should be regarded as a code template only.
        /// It will require modifications to work:
        /// - It may require correct/in-range values for request initialization.
        /// - It may require specifying regional endpoints when creating the service client as shown in
        ///   https://cloud.google.com/dotnet/docs/reference/help/client-configuration#endpoint.
        /// </remarks>
        public async Task MethodDefaultValuesAsync()
        {
            // Create client
            SnippetsClient snippetsClient = await SnippetsClient.CreateAsync();
            // Initialize request argument(s)
            IEnumerable<double> repeatedDouble = new double[] { 0, };
            IEnumerable<DefaultValuesRequest.Types.NestedMessage> repeatedNestedMessage = new DefaultValuesRequest.Types.NestedMessage[]
            {
                new DefaultValuesRequest.Types.NestedMessage(),
            };
            IEnumerable<string> repeatedResourceName = new string[]
            {
                "items/[ITEM_ID]/parts/[PART_ID]",
            };
            // Make the request
            Response response = await snippetsClient.MethodDefaultValuesAsync(repeatedDouble, repeatedNestedMessage, repeatedResourceName);
        }
    }
    // [END snippets_generated_Snippets_MethodDefaultValues_async_flattened]
}
