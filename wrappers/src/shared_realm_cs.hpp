////////////////////////////////////////////////////////////////////////////
//
// Copyright 2016 Realm Inc.
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

#ifndef SHARED_REALM_CS_HPP
#define SHARED_REALM_CS_HPP

#include "schema_cs.hpp"
#include "sync_session_cs.hpp"

#include <realm/object-store/shared_realm.hpp>
#include <realm/object-store/binding_context.hpp>
#include <realm/object-store/object_accessor.hpp>
#include <realm/object-store/sync/sync_manager.hpp>
#include <realm/object-store/sync/sync_session.hpp>
#include <realm/sync/config.hpp>

using SharedSyncUser = std::shared_ptr<SyncUser>;

using namespace realm;
using namespace realm::binding;

class ManagedExceptionDuringMigration : public std::runtime_error {
public:
    ManagedExceptionDuringMigration() : std::runtime_error("Uncaught .NET exception during Realm migration") {
    }
};

struct Configuration
{
    uint16_t* path;
    size_t path_len;
    
    uint16_t* fallback_path;
    size_t fallback_path_len;

    bool read_only;
    
    bool in_memory;
    
    bool delete_if_migration_needed;
    
    uint64_t schema_version;
    
    void* managed_migration_handle;
    
    void* managed_should_compact_delegate;
    
    bool enable_cache;
    uint64_t max_number_of_active_versions;
};

struct SyncConfiguration
{
    SharedSyncUser* user;

    uint16_t* partition;
    size_t partition_len;

    SyncSessionStopPolicy session_stop_policy;

    SchemaMode schema_mode;

    bool is_flexible_sync;
};

inline const TableRef get_table(const SharedRealm& realm, TableKey table_key)
{
    return realm->read_group().get_table(table_key);
}

namespace realm {
namespace binding {
    
    class CSharpBindingContext: public BindingContext {
    public:
        CSharpBindingContext(void* managed_state_handle);
        void did_change(std::vector<CSharpBindingContext::ObserverState> const& observed, std::vector<void*> const& invalidated, bool version_changed) override;
        
        void* get_managed_state_handle()
        {
            return m_managed_state_handle;
        }

        // TODO: this should go away once https://github.com/realm/realm-core/issues/4584 is resolved
        Schema m_realm_schema;

    private:
        void* m_managed_state_handle;

        ~CSharpBindingContext();
    };

    void log_message(std::string message, util::Logger::Level level = util::Logger::Level::info);
}
    
}

#endif /* defined(SHARED_REALM_CS_HPP) */
