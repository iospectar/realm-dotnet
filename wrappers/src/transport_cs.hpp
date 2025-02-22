////////////////////////////////////////////////////////////////////////////
//
// Copyright 2020 Realm Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
////////////////////////////////////////////////////////////////////////////

#ifndef TRANSPORT_CS_HPP
#define TRANSPORT_CS_HPP

#include <realm/object-store/sync/generic_network_transport.hpp>

using namespace realm::app;

namespace realm {
namespace binding {
    extern std::shared_ptr<GenericNetworkTransport> s_transport;
}
}

#endif /* defined(TRANSPORT_CS_HPP) */
