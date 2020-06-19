#include <amxx/api.h>
#include <$pluginprojectdirname$/regamedll_api.h>

AmxxStatus on_amxx_attach()
{
	if (!RegamedllApi::init()) {
		return AmxxStatus::Failed;
	}

	return AmxxStatus::Ok;
}

void on_amxx_detach()
{
}
