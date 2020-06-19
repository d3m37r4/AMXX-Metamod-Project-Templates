#include <amxx/api.h>
#include <$pluginprojectdirname$/rehlds_api.h>

AmxxStatus on_amxx_attach()
{
	if (!RehldsApi::init()) {
		return AmxxStatus::Failed;
	}

	return AmxxStatus::Ok;
}

void on_amxx_detach()
{
}
