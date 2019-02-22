﻿// Copyright 2018 Google Inc. All Rights Reserved.
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

using Google.Api.Gax;
using Google.Api.Gax.Grpc;
using Google.Api.Generator.ProtoUtils;
using Google.Api.Generator.RoslynUtils;
using Google.Api.Generator.Utils;
using Google.LongRunning;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Google.Api.Generator.RoslynUtils.Modifier;
using static Google.Api.Generator.RoslynUtils.RoslynBuilder;

namespace Google.Api.Generator.Generation
{
    internal class SnippetCodeGenerator
    {
        public static CompilationUnitSyntax Generate(SourceFileContext ctx, ServiceDetails svc) =>
            new SnippetCodeGenerator(ctx, svc).Generate();

        private SnippetCodeGenerator(SourceFileContext ctx, ServiceDetails svc) => (_ctx, _svc) = (ctx, svc);

        private readonly SourceFileContext _ctx;
        private readonly ServiceDetails _svc;

        private CompilationUnitSyntax Generate()
        {
            var ns = Namespace(_svc.Namespace);
            using (_ctx.InNamespace(ns))
            {
                var cls = Class(Public | Sealed, _svc.SnippetsTyp)
                    .WithXmlDoc(XmlDoc.Summary("Generated snippets."));
                using (_ctx.InClass(cls))
                {
                    cls = cls.AddMembers(GenerateMethods().ToArray());
                }
                ns = ns.AddMembers(cls);
            }
            return _ctx.CreateCompilationUnit(ns);
        }

        private IEnumerable<MethodDeclarationSyntax> GenerateMethods()
        {
            foreach (var method in _svc.Methods)
            {
                var methodDef = new MethodDef(_ctx, _svc, method);
                switch (method)
                {
                    case MethodDetails.Normal _:
                        yield return methodDef.SyncRequestMethod;
                        yield return methodDef.AsyncRequestMethod;
                        foreach (var signature in methodDef.Signatures)
                        {
                            yield return signature.SyncMethod;
                            yield return signature.AsyncMethod;
                            if (signature.HasResourceNames)
                            {
                                yield return signature.SyncMethodResourceNames;
                                yield return signature.AsyncMethodResourceNames;
                            }
                        }
                        break;
                    case MethodDetails.Lro _:
                        yield return methodDef.SyncLroRequestMethod;
                        yield return methodDef.AsyncLroRequestMethod;
                        foreach (var signature in methodDef.Signatures)
                        {
                            yield return signature.SyncLroMethod;
                            yield return signature.AsyncLroMethod;
                            if (signature.HasResourceNames)
                            {
                                yield return signature.SyncLroMethodResourceNames;
                                yield return signature.AsyncLroMethodResourceNames;
                            }
                        }
                        break;
                }
            }
        }

        private class MethodDef
        {
            public MethodDef(SourceFileContext ctx, ServiceDetails svc, MethodDetails method) =>
                (Ctx, Svc, Method) = (ctx, svc, method);

            private SourceFileContext Ctx { get; }
            private ServiceDetails Svc { get; }
            private MethodDetails Method { get; }
            private MethodDetails.Lro MethodLro => (MethodDetails.Lro)Method;

            private LocalDeclarationStatementSyntax Client => Local(Ctx.Type(Svc.ClientAbstractTyp), Svc.SnippetsClientName);
            private LocalDeclarationStatementSyntax Request => Local(Ctx.Type(Method.RequestTyp), "request");
            private LocalDeclarationStatementSyntax Response => Local(Ctx.Type(Method.ResponseTyp), "response");
            private LocalDeclarationStatementSyntax LroResponse => Local(Ctx.Type(MethodLro.OperationTyp), "response");
            private LocalDeclarationStatementSyntax LroCompletedResponse => Local(Ctx.Type(MethodLro.OperationTyp), "completedResponse");
            private LocalDeclarationStatementSyntax LroResult => Local(Ctx.Type(MethodLro.OperationResponseTyp), "result");
            private LocalDeclarationStatementSyntax LroOperationName => Local(Ctx.Type<string>(), "operationName");
            private LocalDeclarationStatementSyntax LroRetrievedResponse => Local(Ctx.Type(MethodLro.OperationTyp), "retrievedResponse");
            private LocalDeclarationStatementSyntax LroRetrievedResult => Local(Ctx.Type(MethodLro.OperationResponseTyp), "retrievedResult");

            private object DefaultValue(FieldDescriptor fieldDesc, bool resourceNameAsString = false)
            {
                var resource = Svc.Catalog.GetResourceDetailsByField(fieldDesc);
                if (resource != null)
                {
                    // TODO: Resource-sets
                    var one = resource.ResourceDefinition.One;
                    object @default;
                    if (resourceNameAsString)
                    {
                        @default = one.IsWildcard ?
                            "a/wildcard/resource" :
                            one.Template.Expand(one.Template.ParameterNames.Select(x => $"[{x.ToUpperInvariant()}]").ToArray());
                    }
                    else
                    {
                        @default = one.IsWildcard ?
                            New(Ctx.Type<UnknownResourceName>())("a/wildcard/resource") :
                            New(Ctx.Type(one.ResourceNameTyp))(one.Template.ParameterNames.Select(x => $"[{x.ToUpperInvariant()}]"));
                    }
                    return fieldDesc.IsRepeated ? CollectionInitializer(@default) : @default;
                }
                else
                {
                    object @default;
                    if (fieldDesc.IsMap)
                    {
                        throw new NotImplementedException("Map types not yet implemented.");
                    }
                    // See https://developers.google.com/protocol-buffers/docs/proto3#scalar
                    // Switch cases are ordered as in this doc. Please do not re-order.
                    switch (fieldDesc.FieldType)
                    {
                        case FieldType.Double: @default = default(double); break;
                        case FieldType.Float: @default = default(float); break;
                        case FieldType.Int32: @default = default(int); break;
                        case FieldType.Int64: @default = default(long); break;
                        case FieldType.UInt32: @default = default(uint); break;
                        case FieldType.UInt64: @default = default(ulong); break;
                        case FieldType.SInt32: @default = default(int); break;
                        case FieldType.SInt64: @default = default(long); break;
                        case FieldType.Fixed32: @default = default(uint); break;
                        case FieldType.Fixed64: @default = default(ulong); break;
                        case FieldType.SFixed32: @default = default(int); break;
                        case FieldType.SFixed64: @default = default(long); break;
                        case FieldType.Bool: @default = default(bool); break;
                        case FieldType.String: @default = ""; break;
                        case FieldType.Bytes: @default = Ctx.Type<ByteString>().Access(nameof(ByteString.Empty)); break;
                        case FieldType.Message: @default = New(Ctx.Type(Typ.Of(fieldDesc.MessageType)))(); break;
                        case FieldType.Enum: @default = Ctx.Type(Typ.Of(fieldDesc.EnumType)).Access(fieldDesc.EnumType.Values.First().CSharpName()); break;
                        default: throw new InvalidOperationException($"Cannot generate default for proto type: {fieldDesc.FieldType}");

                    }
                    return fieldDesc.IsRepeated ? CollectionInitializer(@default) : @default;
                }
            }

            private IEnumerable<ObjectInitExpr> InitRequest()
            {
                foreach (var fieldDesc in Method.RequestMessageDesc.Fields.InFieldNumberOrder())
                {
                    yield return new ObjectInitExpr(
                        Svc.Catalog.GetResourceDetailsByField(fieldDesc)?.ResourcePropertyName ?? fieldDesc.CSharpPropertyName(),
                        DefaultValue(fieldDesc));
                }
            }

            private MethodDeclarationSyntax Sync(string methodName, IEnumerable<Typ> snippetTyps, object initRequest, object makeRequest) =>
                Method(Public, VoidType, methodName)()
                    .WithBody(
                        $"// Snippet: {Method.SyncMethodName}({string.Join("", snippetTyps.Select(x => $"{x.Name}, "))}{nameof(CallSettings)})",
                        "// Create client",
                        Client.WithInitializer(Ctx.Type(Svc.ClientAbstractTyp).Call("Create")()),
                        snippetTyps.Any() ? "// Initialize request argument(s)" : null,
                        initRequest,
                        "// Make the request",
                        makeRequest,
                        "// End snippet")
                    .WithXmlDoc(XmlDoc.Summary($"Snippet for {Method.SyncMethodName}"));

            private MethodDeclarationSyntax Async(string methodName, IEnumerable<Typ> snippetTyps, object initRequest, object makeRequest) =>
                Method(Public | Modifier.Async, Ctx.Type<Task>(), methodName)()
                    .WithBody(
                        $"// Snippet: {Method.AsyncMethodName}({string.Join("", snippetTyps.Select(x => $"{x.Name}, "))}{nameof(CallSettings)})",
                        $"// Additional: {Method.AsyncMethodName}({string.Join("", snippetTyps.Select(x => $"{x.Name}, "))}{nameof(CancellationToken)})",
                        "// Create client",
                        Client.WithInitializer(Await(Ctx.Type(Svc.ClientAbstractTyp).Call("CreateAsync")())),
                        "// Initialize request argument(s)",
                        initRequest,
                        "// Make the request",
                        makeRequest,
                        "// End snippet")
                    .WithXmlDoc(XmlDoc.Summary($"Snippet for {Method.AsyncMethodName}"));

            private MethodDeclarationSyntax SyncLro(string methodName, IEnumerable<Typ> snippetTyps, object initRequest, object makeRequest) =>
                Method(Public, VoidType, methodName)()
                    .WithBody(
                        $"// Snippet: {Method.SyncMethodName}({string.Join("", snippetTyps.Select(x => $"{x.Name}, "))}{nameof(CallSettings)})",
                        "// Create client",
                        Client.WithInitializer(Ctx.Type(Svc.ClientAbstractTyp).Call("Create")()),
                        "// Initialize request argument(s)",
                        initRequest,
                        "// Make the request",
                        makeRequest,
                        BlankLine,
                        "// Poll until the returned long-running operation is complete",
                        LroCompletedResponse.WithInitializer(LroResponse.Call(nameof(Operation<ProtoMsg, ProtoMsg>.PollUntilCompleted))()),
                        "// Retrieve the operation result",
                        LroResult.WithInitializer(LroCompletedResponse.Access(nameof(Operation<ProtoMsg, ProtoMsg>.Result))),
                        BlankLine,
                        "// Or get the name of the operation",
                        LroOperationName.WithInitializer(LroResponse.Access(nameof(Operation<ProtoMsg, ProtoMsg>.Name))),
                        "// This name can be stored, then the long-running operation retrieved later by name",
                        LroRetrievedResponse.WithInitializer(Client.Call(MethodLro.SyncPollMethodName)(LroOperationName)),
                        "// Check if the retrieved long-running operation has completed",
                        If(LroRetrievedResponse.Access(nameof(Operation<ProtoMsg, ProtoMsg>.IsCompleted)))
                            .Then(
                                "// If it has completed, then access the result",
                                LroRetrievedResult.WithInitializer(LroRetrievedResponse.Access(nameof(Operation<ProtoMsg, ProtoMsg>.Result)))),
                        "// End snippet")
                    .WithXmlDoc(XmlDoc.Summary($"Snippet for {Method.SyncMethodName}"));

            private MethodDeclarationSyntax AsyncLro(string methodName, IEnumerable<Typ> snippetTyps, object initRequest, object makeRequest) =>
                Method(Public | Modifier.Async, Ctx.Type<Task>(), methodName)()
                    .WithBody(
                        $"// Snippet: {Method.AsyncMethodName}({string.Join("", snippetTyps.Select(x => $"{x.Name}, "))}{nameof(CallSettings)})",
                        $"// Additional: {Method.AsyncMethodName}({string.Join("", snippetTyps.Select(x => $"{x.Name}, "))}{nameof(CancellationToken)})",
                        "// Create client",
                        Client.WithInitializer(Await(Ctx.Type(Svc.ClientAbstractTyp).Call("CreateAsync")())),
                        "// Initialize request argument(s)",
                        initRequest,
                        "// Make the request",
                        makeRequest,
                        BlankLine,
                        "// Poll until the returned long-running operation is complete",
                        LroCompletedResponse.WithInitializer(Await(LroResponse.Call(nameof(Operation<ProtoMsg, ProtoMsg>.PollUntilCompletedAsync))())),
                        "// Retrieve the operation result",
                        LroResult.WithInitializer(LroCompletedResponse.Access(nameof(Operation<ProtoMsg, ProtoMsg>.Result))),
                        BlankLine,
                        "// Or get the name of the operation",
                        LroOperationName.WithInitializer(LroResponse.Access(nameof(Operation<ProtoMsg, ProtoMsg>.Name))),
                        "// This name can be stored, then the long-running operation retrieved later by name",
                        LroRetrievedResponse.WithInitializer(Await(Client.Call(MethodLro.AsyncPollMethodName)(LroOperationName))),
                        "// Check if the retrieved long-running operation has completed",
                        If(LroRetrievedResponse.Access(nameof(Operation<ProtoMsg, ProtoMsg>.IsCompleted)))
                            .Then(
                                "// If it has completed, then access the result",
                                LroRetrievedResult.WithInitializer(LroRetrievedResponse.Access(nameof(Operation<ProtoMsg, ProtoMsg>.Result)))),
                        "// End snippet")
                    .WithXmlDoc(XmlDoc.Summary($"Snippet for {Method.AsyncMethodName}"));

            public MethodDeclarationSyntax SyncRequestMethod => Sync(Method.SyncSnippetMethodName, new[] { Method.RequestTyp },
                Request.WithInitializer(New(Ctx.Type(Method.RequestTyp))().WithInitializer(InitRequest().ToArray())),
                Response.WithInitializer(Client.Call(Method.SyncMethodName)(Request)));

            public MethodDeclarationSyntax AsyncRequestMethod => Async(Method.AsyncSnippetMethodName, new[] { Method.RequestTyp },
                Request.WithInitializer(New(Ctx.Type(Method.RequestTyp))().WithInitializer(InitRequest().ToArray())),
                Response.WithInitializer(Await(Client.Call(Method.AsyncMethodName)(Request))));

            public MethodDeclarationSyntax SyncLroRequestMethod => SyncLro(Method.SyncSnippetMethodName, new[] { Method.RequestTyp },
                Request.WithInitializer(New(Ctx.Type(Method.RequestTyp))().WithInitializer(InitRequest().ToArray())),
                LroResponse.WithInitializer(Client.Call(Method.SyncMethodName)(Request)));

            public MethodDeclarationSyntax AsyncLroRequestMethod => AsyncLro(Method.AsyncSnippetMethodName, new[] { Method.RequestTyp },
                Request.WithInitializer(New(Ctx.Type(Method.RequestTyp))().WithInitializer(InitRequest().ToArray())),
                LroResponse.WithInitializer(Await(Client.Call(Method.AsyncMethodName)(Request))));

            public class Signature
            {
                // TODO: Support resource-sets.

                public Signature(MethodDef def, MethodDetails.Signature sig, int? index) => (_def, _sig, _index) = (def, sig, index);

                private MethodDef _def;
                private MethodDetails.Signature _sig;
                private int? _index;

                private ServiceDetails Svc => _def.Svc;
                private SourceFileContext Ctx => _def.Ctx;
                private MethodDetails Method => _def.Method;

                private string SyncMethodName => Method.SyncMethodName + (_index is int index ? (index + 1).ToString() : "");
                private string AsyncMethodName => $"{Method.AsyncMethodName.Substring(0, Method.AsyncMethodName.Length - 5)}{(_index is int index ? (index + 1).ToString() : "")}Async";
                private string SyncResourceNameMethodName => $"{SyncMethodName}_ResourceNames";
                private string AsyncResourceNameMethodName => $"{AsyncMethodName}_ResourceNames";

                private IEnumerable<LocalDeclarationStatementSyntax> InitRequestArgs(bool resourceNameAsString) =>
                    _sig.Fields.Select(f =>
                        Local(Ctx.Type(resourceNameAsString ? f.Typ : f.FieldResource?.ResourceDefinition.One.ResourceNameTyp ?? f.Typ), f.FieldName)
                        .WithInitializer(_def.DefaultValue(f.Desc, resourceNameAsString)));
                private IEnumerable<LocalDeclarationStatementSyntax> InitRequestArgsNormal => InitRequestArgs(resourceNameAsString: true);
                private IEnumerable<LocalDeclarationStatementSyntax> InitRequestArgsResourceNames => InitRequestArgs(resourceNameAsString: false);

                private IEnumerable<Typ> SnippetCommentResourceNameArgs => _sig.Fields.Select(f => f.FieldResource?.ResourceDefinition.One.ResourceNameTyp ?? f.Typ);

                public bool HasResourceNames => _sig.Fields.Any(x => x.FieldResource != null);

                public MethodDeclarationSyntax SyncMethod => _def.Sync(SyncMethodName, _sig.Fields.Select(f => f.Typ),
                    InitRequestArgsNormal, _def.Response.WithInitializer(_def.Client.Call(Method.SyncMethodName)(InitRequestArgsNormal.ToArray())));

                public MethodDeclarationSyntax AsyncMethod => _def.Async(AsyncMethodName, _sig.Fields.Select(f => f.Typ),
                    InitRequestArgsNormal, _def.Response.WithInitializer(Await(_def.Client.Call(Method.AsyncMethodName)(InitRequestArgsNormal.ToArray()))));

                public MethodDeclarationSyntax SyncMethodResourceNames => _def.Sync(SyncResourceNameMethodName, SnippetCommentResourceNameArgs,
                    InitRequestArgsResourceNames, _def.Response.WithInitializer(_def.Client.Call(Method.SyncMethodName)(InitRequestArgsResourceNames.ToArray())));

                public MethodDeclarationSyntax AsyncMethodResourceNames => _def.Async(AsyncResourceNameMethodName, SnippetCommentResourceNameArgs,
                    InitRequestArgsResourceNames, _def.Response.WithInitializer(Await(_def.Client.Call(Method.AsyncMethodName)(InitRequestArgsResourceNames.ToArray()))));

                public MethodDeclarationSyntax SyncLroMethod => _def.SyncLro(SyncMethodName, _sig.Fields.Select(f => f.Typ),
                    InitRequestArgsNormal, _def.LroResponse.WithInitializer(_def.Client.Call(Method.SyncMethodName)(InitRequestArgsNormal.ToArray())));

                public MethodDeclarationSyntax AsyncLroMethod => _def.AsyncLro(AsyncMethodName, _sig.Fields.Select(f => f.Typ),
                    InitRequestArgsNormal, _def.LroResponse.WithInitializer(Await(_def.Client.Call(Method.AsyncMethodName)(InitRequestArgsNormal.ToArray()))));

                public MethodDeclarationSyntax SyncLroMethodResourceNames => _def.SyncLro(SyncResourceNameMethodName, SnippetCommentResourceNameArgs,
                    InitRequestArgsResourceNames, _def.LroResponse.WithInitializer(_def.Client.Call(Method.SyncMethodName)(InitRequestArgsResourceNames.ToArray())));

                public MethodDeclarationSyntax AsyncLroMethodResourceNames => _def.AsyncLro(AsyncResourceNameMethodName, SnippetCommentResourceNameArgs,
                    InitRequestArgsResourceNames, _def.LroResponse.WithInitializer(Await(_def.Client.Call(Method.AsyncMethodName)(InitRequestArgsResourceNames.ToArray()))));
            }

            public IEnumerable<Signature> Signatures => Method.Signatures.Select((sig, i) => new Signature(this, sig, Method.Signatures.Count > 1 ? i : (int?)null));
        }
    }
}