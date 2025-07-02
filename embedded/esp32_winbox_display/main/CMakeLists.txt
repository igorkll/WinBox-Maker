file(GLOB_RECURSE FILES "*.c")

idf_component_register(
	SRCS
		${FILES}

    INCLUDE_DIRS
		"."
)

fatfs_create_spiflash_image(storage ../storage FLASH_IN_PROJECT)