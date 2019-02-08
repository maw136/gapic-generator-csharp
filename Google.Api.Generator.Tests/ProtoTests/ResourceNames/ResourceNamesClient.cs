﻿using gax = Google.Api.Gax;
using gaxgrpc = Google.Api.Gax.Grpc;
using linq = System.Linq;
using proto = Google.Protobuf;
using scg = System.Collections.Generic;
using st = System.Threading;
using stt = System.Threading.Tasks;
using sys = System;

namespace Testing.Resourcenames
{
    public abstract class ResourceNamesClient
    {
        public Response SimpleMethod(SimpleResource request, gaxgrpc::CallSettings callSettings) => throw new sys::NotImplementedException();
        public stt::Task<Response> SimpleMethodAsync(SimpleResource request, gaxgrpc::CallSettings callSettings) => throw new sys::NotImplementedException();

        // TEST_START
        /// <summary>
        /// Test top-level resource definition.
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <param name="callSettings">If not null, applies overrides to this RPC call.</param>
        /// <returns>The RPC response.</returns>
        public virtual Response SimpleMethod(string name, gaxgrpc::CallSettings callSettings = null) =>
            SimpleMethod(new SimpleResource
            {
                Name = name ?? "",
            }, callSettings);

        /// <summary>
        /// Test top-level resource definition.
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <param name="callSettings">If not null, applies overrides to this RPC call.</param>
        /// <returns>A Task containing the RPC response.</returns>
        public virtual stt::Task<Response> SimpleMethodAsync(string name, gaxgrpc::CallSettings callSettings = null) =>
            SimpleMethodAsync(new SimpleResource
            {
                Name = name ?? "",
            }, callSettings);

        /// <summary>
        /// Test top-level resource definition.
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <param name="cancellationToken">A <see cref="st::CancellationToken"/> to use for this RPC.</param>
        /// <returns>A Task containing the RPC response.</returns>
        public virtual stt::Task<Response> SimpleMethodAsync(string name, st::CancellationToken cancellationToken) =>
            SimpleMethodAsync(name, gaxgrpc::CallSettings.FromCancellationToken(cancellationToken));

        /// <summary>
        /// Test top-level resource definition.
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <param name="callSettings">If not null, applies overrides to this RPC call.</param>
        /// <returns>The RPC response.</returns>
        public virtual Response SimpleMethod(SimpleResourceName name, gaxgrpc::CallSettings callSettings = null) =>
            SimpleMethod(new SimpleResource
            {
                SimpleResourceName = name,
            }, callSettings);

        /// <summary>
        /// Test top-level resource definition.
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <param name="callSettings">If not null, applies overrides to this RPC call.</param>
        /// <returns>A Task containing the RPC response.</returns>
        public virtual stt::Task<Response> SimpleMethodAsync(SimpleResourceName name, gaxgrpc::CallSettings callSettings = null) =>
            SimpleMethodAsync(new SimpleResource
            {
                SimpleResourceName = name,
            }, callSettings);

        /// <summary>
        /// Test top-level resource definition.
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <param name="cancellationToken">A <see cref="st::CancellationToken"/> to use for this RPC.</param>
        /// <returns>A Task containing the RPC response.</returns>
        public virtual stt::Task<Response> SimpleMethodAsync(SimpleResourceName name, st::CancellationToken cancellationToken) =>
            SimpleMethodAsync(name, gaxgrpc::CallSettings.FromCancellationToken(cancellationToken));
        // TEST_END

        public Response SimpleInlineMethod(SimpleInlineResource request, gaxgrpc::CallSettings callSettings) => throw new sys::NotImplementedException();
        public stt::Task<Response> SimpleInlineMethodAsync(SimpleInlineResource request, gaxgrpc::CallSettings callSettings) => throw new sys::NotImplementedException();

        // TEST_START
        /// <summary>
        /// Test resource definition inlined in the proto message.
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <param name="callSettings">If not null, applies overrides to this RPC call.</param>
        /// <returns>The RPC response.</returns>
        public virtual Response SimpleInlineMethod(string name, gaxgrpc::CallSettings callSettings = null) =>
            SimpleInlineMethod(new SimpleInlineResource
            {
                Name = name ?? "",
            }, callSettings);

        /// <summary>
        /// Test resource definition inlined in the proto message.
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <param name="callSettings">If not null, applies overrides to this RPC call.</param>
        /// <returns>A Task containing the RPC response.</returns>
        public virtual stt::Task<Response> SimpleInlineMethodAsync(string name, gaxgrpc::CallSettings callSettings = null) =>
            SimpleInlineMethodAsync(new SimpleInlineResource
            {
                Name = name ?? "",
            }, callSettings);

        /// <summary>
        /// Test resource definition inlined in the proto message.
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <param name="cancellationToken">A <see cref="st::CancellationToken"/> to use for this RPC.</param>
        /// <returns>A Task containing the RPC response.</returns>
        public virtual stt::Task<Response> SimpleInlineMethodAsync(string name, st::CancellationToken cancellationToken) =>
            SimpleInlineMethodAsync(name, gaxgrpc::CallSettings.FromCancellationToken(cancellationToken));

        /// <summary>
        /// Test resource definition inlined in the proto message.
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <param name="callSettings">If not null, applies overrides to this RPC call.</param>
        /// <returns>The RPC response.</returns>
        public virtual Response SimpleInlineMethod(SimpleInlineResourceName name, gaxgrpc::CallSettings callSettings = null) =>
            SimpleInlineMethod(new SimpleInlineResource
            {
                SimpleInlineResourceName = name,
            }, callSettings);

        /// <summary>
        /// Test resource definition inlined in the proto message.
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <param name="callSettings">If not null, applies overrides to this RPC call.</param>
        /// <returns>A Task containing the RPC response.</returns>
        public virtual stt::Task<Response> SimpleInlineMethodAsync(SimpleInlineResourceName name, gaxgrpc::CallSettings callSettings = null) =>
            SimpleInlineMethodAsync(new SimpleInlineResource
            {
                SimpleInlineResourceName = name,
            }, callSettings);

        /// <summary>
        /// Test resource definition inlined in the proto message.
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <param name="cancellationToken">A <see cref="st::CancellationToken"/> to use for this RPC.</param>
        /// <returns>A Task containing the RPC response.</returns>
        public virtual stt::Task<Response> SimpleInlineMethodAsync(SimpleInlineResourceName name, st::CancellationToken cancellationToken) =>
            SimpleInlineMethodAsync(name, gaxgrpc::CallSettings.FromCancellationToken(cancellationToken));
        // TEST_END

        public Response SimpleRepeatedMethod(SimpleRepeatedResource request, gaxgrpc::CallSettings callSettings) => throw new sys::NotImplementedException();
        public stt::Task<Response> SimpleRepeatedMethodAsync(SimpleRepeatedResource request, gaxgrpc::CallSettings callSettings) => throw new sys::NotImplementedException();

        // TEST_START
        /// <summary>
        /// Test repeated resource.
        /// </summary>
        /// <param name="names">
        /// </param>
        /// <param name="callSettings">If not null, applies overrides to this RPC call.</param>
        /// <returns>The RPC response.</returns>
        public virtual Response SimpleRepeatedMethod(scg::IEnumerable<string> names, gaxgrpc::CallSettings callSettings = null) =>
            SimpleRepeatedMethod(new SimpleRepeatedResource
            {
                Names =
                {
                    names ?? linq::Enumerable.Empty<string>(),
                },
            }, callSettings);

        /// <summary>
        /// Test repeated resource.
        /// </summary>
        /// <param name="names">
        /// </param>
        /// <param name="callSettings">If not null, applies overrides to this RPC call.</param>
        /// <returns>A Task containing the RPC response.</returns>
        public virtual stt::Task<Response> SimpleRepeatedMethodAsync(scg::IEnumerable<string> names, gaxgrpc::CallSettings callSettings = null) =>
            SimpleRepeatedMethodAsync(new SimpleRepeatedResource
            {
                Names =
                {
                    names ?? linq::Enumerable.Empty<string>(),
                },
            }, callSettings);

        /// <summary>
        /// Test repeated resource.
        /// </summary>
        /// <param name="names">
        /// </param>
        /// <param name="cancellationToken">A <see cref="st::CancellationToken"/> to use for this RPC.</param>
        /// <returns>A Task containing the RPC response.</returns>
        public virtual stt::Task<Response> SimpleRepeatedMethodAsync(scg::IEnumerable<string> names, st::CancellationToken cancellationToken) =>
            SimpleRepeatedMethodAsync(names, gaxgrpc::CallSettings.FromCancellationToken(cancellationToken));

        /// <summary>
        /// Test repeated resource.
        /// </summary>
        /// <param name="names">
        /// </param>
        /// <param name="callSettings">If not null, applies overrides to this RPC call.</param>
        /// <returns>The RPC response.</returns>
        public virtual Response SimpleRepeatedMethod(scg::IEnumerable<SimpleResourceName> names, gaxgrpc::CallSettings callSettings = null) =>
            SimpleRepeatedMethod(new SimpleRepeatedResource
            {
                SimpleResourceNames =
                {
                    names ?? linq::Enumerable.Empty<SimpleResourceName>(),
                },
            }, callSettings);

        /// <summary>
        /// Test repeated resource.
        /// </summary>
        /// <param name="names">
        /// </param>
        /// <param name="callSettings">If not null, applies overrides to this RPC call.</param>
        /// <returns>A Task containing the RPC response.</returns>
        public virtual stt::Task<Response> SimpleRepeatedMethodAsync(scg::IEnumerable<SimpleResourceName> names, gaxgrpc::CallSettings callSettings = null) =>
            SimpleRepeatedMethodAsync(new SimpleRepeatedResource
            {
                SimpleResourceNames =
                {
                    names ?? linq::Enumerable.Empty<SimpleResourceName>(),
                },
            }, callSettings);

        /// <summary>
        /// Test repeated resource.
        /// </summary>
        /// <param name="names">
        /// </param>
        /// <param name="cancellationToken">A <see cref="st::CancellationToken"/> to use for this RPC.</param>
        /// <returns>A Task containing the RPC response.</returns>
        public virtual stt::Task<Response> SimpleRepeatedMethodAsync(scg::IEnumerable<SimpleResourceName> names, st::CancellationToken cancellationToken) =>
            SimpleRepeatedMethodAsync(names, gaxgrpc::CallSettings.FromCancellationToken(cancellationToken));
        // TEST_END
    }
}
