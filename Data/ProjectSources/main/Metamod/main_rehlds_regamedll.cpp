#include <metamod/api.h>
#include <$pluginprojectdirname$/rehlds_api.h>
#include <$pluginprojectdirname$/regamedll_api.h>

MetamodStatus on_meta_attach()
{
	if (!RehldsApi::init() || !RegamedllApi::init()) {
		return MetamodStatus::Failed;
	}

	return MetamodStatus::Ok;
}

void on_meta_detach()
{
}
