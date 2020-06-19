#include <metamod/api.h>
#include <$pluginprojectdirname$/rehlds_api.h>

MetamodStatus on_meta_attach()
{
	if (!RehldsApi::init()) {
		return MetamodStatus::Failed;
	}

	return MetamodStatus::Ok;
}

void on_meta_detach()
{
}
