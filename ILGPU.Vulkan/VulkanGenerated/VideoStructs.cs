using System.Runtime.InteropServices;

#nullable disable

namespace GPUVulkan
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH264SpsVuiFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH264HrdParameters
	{
		public byte cpb_cnt_minus1;
		public byte bit_rate_scale;
		public byte cpb_size_scale;
		public byte reserved1;
		public fixed uint bit_rate_value_minus1[32];
		public fixed uint cpb_size_value_minus1[32];
		public fixed byte cbr_flag[32];
		public uint initial_cpb_removal_delay_length_minus1;
		public uint cpb_removal_delay_length_minus1;
		public uint dpb_output_delay_length_minus1;
		public uint time_offset_length;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH264SequenceParameterSetVui
	{
		public StdVideoH264SpsVuiFlags flags;
		public StdVideoH264AspectRatioIdc aspect_ratio_idc;
		public ushort sar_width;
		public ushort sar_height;
		public byte video_format;
		public byte colour_primaries;
		public byte transfer_characteristics;
		public byte matrix_coefficients;
		public uint num_units_in_tick;
		public uint time_scale;
		public byte max_num_reorder_frames;
		public byte max_dec_frame_buffering;
		public byte chroma_sample_loc_type_top_field;
		public byte chroma_sample_loc_type_bottom_field;
		public uint reserved1;
		public StdVideoH264HrdParameters* pHrdParameters;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH264SpsFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH264ScalingLists
	{
		public ushort scaling_list_present_mask;
		public ushort use_default_scaling_matrix_mask;
		public fixed byte ScalingList4x4[96];
		public fixed byte ScalingList8x8[384];
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH264SequenceParameterSet
	{
		public StdVideoH264SpsFlags flags;
		public StdVideoH264ProfileIdc profile_idc;
		public StdVideoH264LevelIdc level_idc;
		public StdVideoH264ChromaFormatIdc chroma_format_idc;
		public byte seq_parameter_set_id;
		public byte bit_depth_luma_minus8;
		public byte bit_depth_chroma_minus8;
		public byte log2_max_frame_num_minus4;
		public StdVideoH264PocType pic_order_cnt_type;
		public int offset_for_non_ref_pic;
		public int offset_for_top_to_bottom_field;
		public byte log2_max_pic_order_cnt_lsb_minus4;
		public byte num_ref_frames_in_pic_order_cnt_cycle;
		public byte max_num_ref_frames;
		public byte reserved1;
		public uint pic_width_in_mbs_minus1;
		public uint pic_height_in_map_units_minus1;
		public uint frame_crop_left_offset;
		public uint frame_crop_right_offset;
		public uint frame_crop_top_offset;
		public uint frame_crop_bottom_offset;
		public uint reserved2;
		public int* pOffsetForRefFrame;
		public StdVideoH264ScalingLists* pScalingLists;
		public StdVideoH264SequenceParameterSetVui* pSequenceParameterSetVui;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH264PpsFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH264PictureParameterSet
	{
		public StdVideoH264PpsFlags flags;
		public byte seq_parameter_set_id;
		public byte pic_parameter_set_id;
		public byte num_ref_idx_l0_default_active_minus1;
		public byte num_ref_idx_l1_default_active_minus1;
		public StdVideoH264WeightedBipredIdc weighted_bipred_idc;
		public sbyte pic_init_qp_minus26;
		public sbyte pic_init_qs_minus26;
		public sbyte chroma_qp_index_offset;
		public sbyte second_chroma_qp_index_offset;
		public StdVideoH264ScalingLists* pScalingLists;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoDecodeH264PictureInfoFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoDecodeH264PictureInfo
	{
		public StdVideoDecodeH264PictureInfoFlags flags;
		public byte seq_parameter_set_id;
		public byte pic_parameter_set_id;
		public byte reserved1;
		public byte reserved2;
		public ushort frame_num;
		public ushort idr_pic_id;
		public fixed int PicOrderCnt[2];
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoDecodeH264ReferenceInfoFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoDecodeH264ReferenceInfo
	{
		public StdVideoDecodeH264ReferenceInfoFlags flags;
		public ushort FrameNum;
		public ushort reserved;
		public fixed int PicOrderCnt[2];
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH264WeightTableFlags
	{
		public uint luma_weight_l0_flag;
		public uint chroma_weight_l0_flag;
		public uint luma_weight_l1_flag;
		public uint chroma_weight_l1_flag;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH264WeightTable
	{
		public StdVideoEncodeH264WeightTableFlags flags;
		public byte luma_log2_weight_denom;
		public byte chroma_log2_weight_denom;
		public fixed sbyte luma_weight_l0[32];
		public fixed sbyte luma_offset_l0[32];
		public fixed sbyte chroma_weight_l0[64];
		public fixed sbyte chroma_offset_l0[64];
		public fixed sbyte luma_weight_l1[32];
		public fixed sbyte luma_offset_l1[32];
		public fixed sbyte chroma_weight_l1[64];
		public fixed sbyte chroma_offset_l1[64];
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH264SliceHeaderFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH264PictureInfoFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH264ReferenceInfoFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH264ReferenceListsInfoFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH264RefListModEntry
	{
		public StdVideoH264ModificationOfPicNumsIdc modification_of_pic_nums_idc;
		public ushort abs_diff_pic_num_minus1;
		public ushort long_term_pic_num;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH264RefPicMarkingEntry
	{
		public StdVideoH264MemMgmtControlOp memory_management_control_operation;
		public ushort difference_of_pic_nums_minus1;
		public ushort long_term_pic_num;
		public ushort long_term_frame_idx;
		public ushort max_long_term_frame_idx_plus1;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH264ReferenceListsInfo
	{
		public StdVideoEncodeH264ReferenceListsInfoFlags flags;
		public byte num_ref_idx_l0_active_minus1;
		public byte num_ref_idx_l1_active_minus1;
		public fixed byte RefPicList0[32];
		public fixed byte RefPicList1[32];
		public byte refList0ModOpCount;
		public byte refList1ModOpCount;
		public byte refPicMarkingOpCount;
		public fixed byte reserved1[7];
		public StdVideoEncodeH264RefListModEntry* pRefList0ModOperations;
		public StdVideoEncodeH264RefListModEntry* pRefList1ModOperations;
		public StdVideoEncodeH264RefPicMarkingEntry* pRefPicMarkingOperations;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH264PictureInfo
	{
		public StdVideoEncodeH264PictureInfoFlags flags;
		public byte seq_parameter_set_id;
		public byte pic_parameter_set_id;
		public ushort idr_pic_id;
		public StdVideoH264PictureType primary_pic_type;
		public uint frame_num;
		public int PicOrderCnt;
		public byte temporal_id;
		public fixed byte reserved1[3];
		public StdVideoEncodeH264ReferenceListsInfo* pRefLists;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH264ReferenceInfo
	{
		public StdVideoEncodeH264ReferenceInfoFlags flags;
		public StdVideoH264PictureType primary_pic_type;
		public uint FrameNum;
		public int PicOrderCnt;
		public ushort long_term_pic_num;
		public ushort long_term_frame_idx;
		public byte temporal_id;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH264SliceHeader
	{
		public StdVideoEncodeH264SliceHeaderFlags flags;
		public uint first_mb_in_slice;
		public StdVideoH264SliceType slice_type;
		public sbyte slice_alpha_c0_offset_div2;
		public sbyte slice_beta_offset_div2;
		public sbyte slice_qp_delta;
		public byte reserved1;
		public StdVideoH264CabacInitIdc cabac_init_idc;
		public StdVideoH264DisableDeblockingFilterIdc disable_deblocking_filter_idc;
		public StdVideoEncodeH264WeightTable* pWeightTable;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH265ProfileTierLevelFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH265ProfileTierLevel
	{
		public StdVideoH265ProfileTierLevelFlags flags;
		public StdVideoH265ProfileIdc general_profile_idc;
		public StdVideoH265LevelIdc general_level_idc;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH265DecPicBufMgr
	{
		public fixed uint max_latency_increase_plus1[7];
		public fixed byte max_dec_pic_buffering_minus1[7];
		public fixed byte max_num_reorder_pics[7];
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH265SubLayerHrdParameters
	{
		public fixed uint bit_rate_value_minus1[32];
		public fixed uint cpb_size_value_minus1[32];
		public fixed uint cpb_size_du_value_minus1[32];
		public fixed uint bit_rate_du_value_minus1[32];
		public uint cbr_flag;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH265HrdFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH265HrdParameters
	{
		public StdVideoH265HrdFlags flags;
		public byte tick_divisor_minus2;
		public byte du_cpb_removal_delay_increment_length_minus1;
		public byte dpb_output_delay_du_length_minus1;
		public byte bit_rate_scale;
		public byte cpb_size_scale;
		public byte cpb_size_du_scale;
		public byte initial_cpb_removal_delay_length_minus1;
		public byte au_cpb_removal_delay_length_minus1;
		public byte dpb_output_delay_length_minus1;
		public fixed byte cpb_cnt_minus1[7];
		public fixed ushort elemental_duration_in_tc_minus1[7];
		public fixed ushort reserved[3];
		public StdVideoH265SubLayerHrdParameters* pSubLayerHrdParametersNal;
		public StdVideoH265SubLayerHrdParameters* pSubLayerHrdParametersVcl;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH265VpsFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH265VideoParameterSet
	{
		public StdVideoH265VpsFlags flags;
		public byte vps_video_parameter_set_id;
		public byte vps_max_sub_layers_minus1;
		public byte reserved1;
		public byte reserved2;
		public uint vps_num_units_in_tick;
		public uint vps_time_scale;
		public uint vps_num_ticks_poc_diff_one_minus1;
		public uint reserved3;
		public StdVideoH265DecPicBufMgr* pDecPicBufMgr;
		public StdVideoH265HrdParameters* pHrdParameters;
		public StdVideoH265ProfileTierLevel* pProfileTierLevel;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH265ScalingLists
	{
		public fixed byte ScalingList4x4[96];
		public fixed byte ScalingList8x8[384];
		public fixed byte ScalingList16x16[384];
		public fixed byte ScalingList32x32[128];
		public fixed byte ScalingListDCCoef16x16[6];
		public fixed byte ScalingListDCCoef32x32[2];
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH265ShortTermRefPicSetFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH265ShortTermRefPicSet
	{
		public StdVideoH265ShortTermRefPicSetFlags flags;
		public uint delta_idx_minus1;
		public ushort use_delta_flag;
		public ushort abs_delta_rps_minus1;
		public ushort used_by_curr_pic_flag;
		public ushort used_by_curr_pic_s0_flag;
		public ushort used_by_curr_pic_s1_flag;
		public ushort reserved1;
		public byte reserved2;
		public byte reserved3;
		public byte num_negative_pics;
		public byte num_positive_pics;
		public fixed ushort delta_poc_s0_minus1[16];
		public fixed ushort delta_poc_s1_minus1[16];
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH265LongTermRefPicsSps
	{
		public uint used_by_curr_pic_lt_sps_flag;
		public fixed uint lt_ref_pic_poc_lsb_sps[32];
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH265SpsVuiFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH265SequenceParameterSetVui
	{
		public StdVideoH265SpsVuiFlags flags;
		public StdVideoH265AspectRatioIdc aspect_ratio_idc;
		public ushort sar_width;
		public ushort sar_height;
		public byte video_format;
		public byte colour_primaries;
		public byte transfer_characteristics;
		public byte matrix_coeffs;
		public byte chroma_sample_loc_type_top_field;
		public byte chroma_sample_loc_type_bottom_field;
		public byte reserved1;
		public byte reserved2;
		public ushort def_disp_win_left_offset;
		public ushort def_disp_win_right_offset;
		public ushort def_disp_win_top_offset;
		public ushort def_disp_win_bottom_offset;
		public uint vui_num_units_in_tick;
		public uint vui_time_scale;
		public uint vui_num_ticks_poc_diff_one_minus1;
		public ushort min_spatial_segmentation_idc;
		public ushort reserved3;
		public byte max_bytes_per_pic_denom;
		public byte max_bits_per_min_cu_denom;
		public byte log2_max_mv_length_horizontal;
		public byte log2_max_mv_length_vertical;
		public StdVideoH265HrdParameters* pHrdParameters;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH265PredictorPaletteEntries
	{
		public fixed ushort PredictorPaletteEntries[384];
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH265SpsFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH265SequenceParameterSet
	{
		public StdVideoH265SpsFlags flags;
		public StdVideoH265ChromaFormatIdc chroma_format_idc;
		public uint pic_width_in_luma_samples;
		public uint pic_height_in_luma_samples;
		public byte sps_video_parameter_set_id;
		public byte sps_max_sub_layers_minus1;
		public byte sps_seq_parameter_set_id;
		public byte bit_depth_luma_minus8;
		public byte bit_depth_chroma_minus8;
		public byte log2_max_pic_order_cnt_lsb_minus4;
		public byte log2_min_luma_coding_block_size_minus3;
		public byte log2_diff_max_min_luma_coding_block_size;
		public byte log2_min_luma_transform_block_size_minus2;
		public byte log2_diff_max_min_luma_transform_block_size;
		public byte max_transform_hierarchy_depth_inter;
		public byte max_transform_hierarchy_depth_intra;
		public byte num_short_term_ref_pic_sets;
		public byte num_long_term_ref_pics_sps;
		public byte pcm_sample_bit_depth_luma_minus1;
		public byte pcm_sample_bit_depth_chroma_minus1;
		public byte log2_min_pcm_luma_coding_block_size_minus3;
		public byte log2_diff_max_min_pcm_luma_coding_block_size;
		public byte reserved1;
		public byte reserved2;
		public byte palette_max_size;
		public byte delta_palette_max_predictor_size;
		public byte motion_vector_resolution_control_idc;
		public byte sps_num_palette_predictor_initializers_minus1;
		public uint conf_win_left_offset;
		public uint conf_win_right_offset;
		public uint conf_win_top_offset;
		public uint conf_win_bottom_offset;
		public StdVideoH265ProfileTierLevel* pProfileTierLevel;
		public StdVideoH265DecPicBufMgr* pDecPicBufMgr;
		public StdVideoH265ScalingLists* pScalingLists;
		public StdVideoH265ShortTermRefPicSet* pShortTermRefPicSet;
		public StdVideoH265LongTermRefPicsSps* pLongTermRefPicsSps;
		public StdVideoH265SequenceParameterSetVui* pSequenceParameterSetVui;
		public StdVideoH265PredictorPaletteEntries* pPredictorPaletteEntries;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH265PpsFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoH265PictureParameterSet
	{
		public StdVideoH265PpsFlags flags;
		public byte pps_pic_parameter_set_id;
		public byte pps_seq_parameter_set_id;
		public byte sps_video_parameter_set_id;
		public byte num_extra_slice_header_bits;
		public byte num_ref_idx_l0_default_active_minus1;
		public byte num_ref_idx_l1_default_active_minus1;
		public sbyte init_qp_minus26;
		public byte diff_cu_qp_delta_depth;
		public sbyte pps_cb_qp_offset;
		public sbyte pps_cr_qp_offset;
		public sbyte pps_beta_offset_div2;
		public sbyte pps_tc_offset_div2;
		public byte log2_parallel_merge_level_minus2;
		public byte log2_max_transform_skip_block_size_minus2;
		public byte diff_cu_chroma_qp_offset_depth;
		public byte chroma_qp_offset_list_len_minus1;
		public fixed sbyte cb_qp_offset_list[6];
		public fixed sbyte cr_qp_offset_list[6];
		public byte log2_sao_offset_scale_luma;
		public byte log2_sao_offset_scale_chroma;
		public sbyte pps_act_y_qp_offset_plus5;
		public sbyte pps_act_cb_qp_offset_plus5;
		public sbyte pps_act_cr_qp_offset_plus3;
		public byte pps_num_palette_predictor_initializers;
		public byte luma_bit_depth_entry_minus8;
		public byte chroma_bit_depth_entry_minus8;
		public byte num_tile_columns_minus1;
		public byte num_tile_rows_minus1;
		public byte reserved1;
		public byte reserved2;
		public fixed ushort column_width_minus1[19];
		public fixed ushort row_height_minus1[21];
		public uint reserved3;
		public StdVideoH265ScalingLists* pScalingLists;
		public StdVideoH265PredictorPaletteEntries* pPredictorPaletteEntries;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoDecodeH265PictureInfoFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoDecodeH265PictureInfo
	{
		public StdVideoDecodeH265PictureInfoFlags flags;
		public byte sps_video_parameter_set_id;
		public byte pps_seq_parameter_set_id;
		public byte pps_pic_parameter_set_id;
		public byte NumDeltaPocsOfRefRpsIdx;
		public int PicOrderCntVal;
		public ushort NumBitsForSTRefPicSetInSlice;
		public ushort reserved;
		public fixed byte RefPicSetStCurrBefore[8];
		public fixed byte RefPicSetStCurrAfter[8];
		public fixed byte RefPicSetLtCurr[8];
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoDecodeH265ReferenceInfoFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoDecodeH265ReferenceInfo
	{
		public StdVideoDecodeH265ReferenceInfoFlags flags;
		public int PicOrderCntVal;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH265WeightTableFlags
	{
		public ushort luma_weight_l0_flag;
		public ushort chroma_weight_l0_flag;
		public ushort luma_weight_l1_flag;
		public ushort chroma_weight_l1_flag;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH265WeightTable
	{
		public StdVideoEncodeH265WeightTableFlags flags;
		public byte luma_log2_weight_denom;
		public sbyte delta_chroma_log2_weight_denom;
		public fixed sbyte delta_luma_weight_l0[15];
		public fixed sbyte luma_offset_l0[15];
		public fixed sbyte delta_chroma_weight_l0[30];
		public fixed sbyte delta_chroma_offset_l0[30];
		public fixed sbyte delta_luma_weight_l1[15];
		public fixed sbyte luma_offset_l1[15];
		public fixed sbyte delta_chroma_weight_l1[30];
		public fixed sbyte delta_chroma_offset_l1[30];
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH265LongTermRefPics
	{
		public byte num_long_term_sps;
		public byte num_long_term_pics;
		public fixed byte lt_idx_sps[32];
		public fixed byte poc_lsb_lt[16];
		public ushort used_by_curr_pic_lt_flag;
		public fixed byte delta_poc_msb_present_flag[48];
		public fixed byte delta_poc_msb_cycle_lt[48];
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH265SliceSegmentHeaderFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH265SliceSegmentHeader
	{
		public StdVideoEncodeH265SliceSegmentHeaderFlags flags;
		public StdVideoH265SliceType slice_type;
		public uint slice_segment_address;
		public byte collocated_ref_idx;
		public byte MaxNumMergeCand;
		public sbyte slice_cb_qp_offset;
		public sbyte slice_cr_qp_offset;
		public sbyte slice_beta_offset_div2;
		public sbyte slice_tc_offset_div2;
		public sbyte slice_act_y_qp_offset;
		public sbyte slice_act_cb_qp_offset;
		public sbyte slice_act_cr_qp_offset;
		public sbyte slice_qp_delta;
		public ushort reserved1;
		public StdVideoEncodeH265WeightTable* pWeightTable;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH265ReferenceListsInfoFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH265ReferenceListsInfo
	{
		public StdVideoEncodeH265ReferenceListsInfoFlags flags;
		public byte num_ref_idx_l0_active_minus1;
		public byte num_ref_idx_l1_active_minus1;
		public fixed byte RefPicList0[15];
		public fixed byte RefPicList1[15];
		public fixed byte list_entry_l0[15];
		public fixed byte list_entry_l1[15];
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH265PictureInfoFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH265PictureInfo
	{
		public StdVideoEncodeH265PictureInfoFlags flags;
		public StdVideoH265PictureType pic_type;
		public byte sps_video_parameter_set_id;
		public byte pps_seq_parameter_set_id;
		public byte pps_pic_parameter_set_id;
		public byte short_term_ref_pic_set_idx;
		public int PicOrderCntVal;
		public byte TemporalId;
		public fixed byte reserved1[7];
		public StdVideoEncodeH265ReferenceListsInfo* pRefLists;
		public StdVideoH265ShortTermRefPicSet* pShortTermRefPicSet;
		public StdVideoEncodeH265LongTermRefPics* pLongTermRefPics;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH265ReferenceInfoFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeH265ReferenceInfo
	{
		public StdVideoEncodeH265ReferenceInfoFlags flags;
		public StdVideoH265PictureType pic_type;
		public int PicOrderCntVal;
		public byte TemporalId;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoVP9ColorConfigFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoVP9ColorConfig
	{
		public StdVideoVP9ColorConfigFlags flags;
		public byte BitDepth;
		public byte subsampling_x;
		public byte subsampling_y;
		public byte reserved1;
		public StdVideoVP9ColorSpace color_space;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoVP9LoopFilterFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoVP9LoopFilter
	{
		public StdVideoVP9LoopFilterFlags flags;
		public byte loop_filter_level;
		public byte loop_filter_sharpness;
		public byte update_ref_delta;
		public fixed sbyte loop_filter_ref_deltas[4];
		public byte update_mode_delta;
		public fixed sbyte loop_filter_mode_deltas[2];
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoVP9SegmentationFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoVP9Segmentation
	{
		public StdVideoVP9SegmentationFlags flags;
		public fixed byte segmentation_tree_probs[7];
		public fixed byte segmentation_pred_prob[3];
		public fixed byte FeatureEnabled[8];
		public fixed short FeatureData[32];
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoDecodeVP9PictureInfoFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoDecodeVP9PictureInfo
	{
		public StdVideoDecodeVP9PictureInfoFlags flags;
		public StdVideoVP9Profile profile;
		public StdVideoVP9FrameType frame_type;
		public byte frame_context_idx;
		public byte reset_frame_context;
		public byte refresh_frame_flags;
		public byte ref_frame_sign_bias_mask;
		public StdVideoVP9InterpolationFilter interpolation_filter;
		public byte base_q_idx;
		public sbyte delta_q_y_dc;
		public sbyte delta_q_uv_dc;
		public sbyte delta_q_uv_ac;
		public byte tile_cols_log2;
		public byte tile_rows_log2;
		public fixed ushort reserved1[3];
		public StdVideoVP9ColorConfig* pColorConfig;
		public StdVideoVP9LoopFilter* pLoopFilter;
		public StdVideoVP9Segmentation* pSegmentation;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoAV1ColorConfigFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoAV1ColorConfig
	{
		public StdVideoAV1ColorConfigFlags flags;
		public byte BitDepth;
		public byte subsampling_x;
		public byte subsampling_y;
		public byte reserved1;
		public StdVideoAV1ColorPrimaries color_primaries;
		public StdVideoAV1TransferCharacteristics transfer_characteristics;
		public StdVideoAV1MatrixCoefficients matrix_coefficients;
		public StdVideoAV1ChromaSamplePosition chroma_sample_position;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoAV1TimingInfoFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoAV1TimingInfo
	{
		public StdVideoAV1TimingInfoFlags flags;
		public uint num_units_in_display_tick;
		public uint time_scale;
		public uint num_ticks_per_picture_minus_1;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoAV1SequenceHeaderFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoAV1SequenceHeader
	{
		public StdVideoAV1SequenceHeaderFlags flags;
		public StdVideoAV1Profile seq_profile;
		public byte frame_width_bits_minus_1;
		public byte frame_height_bits_minus_1;
		public ushort max_frame_width_minus_1;
		public ushort max_frame_height_minus_1;
		public byte delta_frame_id_length_minus_2;
		public byte additional_frame_id_length_minus_1;
		public byte order_hint_bits_minus_1;
		public byte seq_force_integer_mv;
		public byte seq_force_screen_content_tools;
		public fixed byte reserved1[5];
		public StdVideoAV1ColorConfig* pColorConfig;
		public StdVideoAV1TimingInfo* pTimingInfo;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoAV1LoopFilterFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoAV1LoopFilter
	{
		public StdVideoAV1LoopFilterFlags flags;
		public fixed byte loop_filter_level[4];
		public byte loop_filter_sharpness;
		public byte update_ref_delta;
		public fixed sbyte loop_filter_ref_deltas[8];
		public byte update_mode_delta;
		public fixed sbyte loop_filter_mode_deltas[2];
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoAV1QuantizationFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoAV1Quantization
	{
		public StdVideoAV1QuantizationFlags flags;
		public byte base_q_idx;
		public sbyte DeltaQYDc;
		public sbyte DeltaQUDc;
		public sbyte DeltaQUAc;
		public sbyte DeltaQVDc;
		public sbyte DeltaQVAc;
		public byte qm_y;
		public byte qm_u;
		public byte qm_v;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoAV1Segmentation
	{
		public fixed byte FeatureEnabled[8];
		public fixed short FeatureData[64];
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoAV1TileInfoFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoAV1TileInfo
	{
		public StdVideoAV1TileInfoFlags flags;
		public byte TileCols;
		public byte TileRows;
		public ushort context_update_tile_id;
		public byte tile_size_bytes_minus_1;
		public fixed byte reserved1[7];
		public ushort* pMiColStarts;
		public ushort* pMiRowStarts;
		public ushort* pWidthInSbsMinus1;
		public ushort* pHeightInSbsMinus1;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoAV1CDEF
	{
		public byte cdef_damping_minus_3;
		public byte cdef_bits;
		public fixed byte cdef_y_pri_strength[8];
		public fixed byte cdef_y_sec_strength[8];
		public fixed byte cdef_uv_pri_strength[8];
		public fixed byte cdef_uv_sec_strength[8];
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoAV1LoopRestoration
	{
		public StdVideoAV1FrameRestorationType FrameRestorationType_0;
		public StdVideoAV1FrameRestorationType FrameRestorationType_1;
		public StdVideoAV1FrameRestorationType FrameRestorationType_2;
		public fixed ushort LoopRestorationSize[3];
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoAV1GlobalMotion
	{
		public fixed byte GmType[8];
		public fixed int gm_params[48];
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoAV1FilmGrainFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoAV1FilmGrain
	{
		public StdVideoAV1FilmGrainFlags flags;
		public byte grain_scaling_minus_8;
		public byte ar_coeff_lag;
		public byte ar_coeff_shift_minus_6;
		public byte grain_scale_shift;
		public ushort grain_seed;
		public byte film_grain_params_ref_idx;
		public byte num_y_points;
		public fixed byte point_y_value[14];
		public fixed byte point_y_scaling[14];
		public byte num_cb_points;
		public fixed byte point_cb_value[10];
		public fixed byte point_cb_scaling[10];
		public byte num_cr_points;
		public fixed byte point_cr_value[10];
		public fixed byte point_cr_scaling[10];
		public fixed sbyte ar_coeffs_y_plus_128[24];
		public fixed sbyte ar_coeffs_cb_plus_128[25];
		public fixed sbyte ar_coeffs_cr_plus_128[25];
		public byte cb_mult;
		public byte cb_luma_mult;
		public ushort cb_offset;
		public byte cr_mult;
		public byte cr_luma_mult;
		public ushort cr_offset;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoDecodeAV1PictureInfoFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoDecodeAV1PictureInfo
	{
		public StdVideoDecodeAV1PictureInfoFlags flags;
		public StdVideoAV1FrameType frame_type;
		public uint current_frame_id;
		public byte OrderHint;
		public byte primary_ref_frame;
		public byte refresh_frame_flags;
		public byte reserved1;
		public StdVideoAV1InterpolationFilter interpolation_filter;
		public StdVideoAV1TxMode TxMode;
		public byte delta_q_res;
		public byte delta_lf_res;
		public fixed byte SkipModeFrame[2];
		public byte coded_denom;
		public fixed byte reserved2[3];
		public fixed byte OrderHints[8];
		public fixed uint expectedFrameId[8];
		public StdVideoAV1TileInfo* pTileInfo;
		public StdVideoAV1Quantization* pQuantization;
		public StdVideoAV1Segmentation* pSegmentation;
		public StdVideoAV1LoopFilter* pLoopFilter;
		public StdVideoAV1CDEF* pCDEF;
		public StdVideoAV1LoopRestoration* pLoopRestoration;
		public StdVideoAV1GlobalMotion* pGlobalMotion;
		public StdVideoAV1FilmGrain* pFilmGrain;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoDecodeAV1ReferenceInfoFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoDecodeAV1ReferenceInfo
	{
		public StdVideoDecodeAV1ReferenceInfoFlags flags;
		public byte frame_type;
		public byte RefFrameSignBias;
		public byte OrderHint;
		public fixed byte SavedOrderHints[8];
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeAV1ExtensionHeader
	{
		public byte temporal_id;
		public byte spatial_id;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeAV1DecoderModelInfo
	{
		public byte buffer_delay_length_minus_1;
		public byte buffer_removal_time_length_minus_1;
		public byte frame_presentation_time_length_minus_1;
		public byte reserved1;
		public uint num_units_in_decoding_tick;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeAV1OperatingPointInfoFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeAV1OperatingPointInfo
	{
		public StdVideoEncodeAV1OperatingPointInfoFlags flags;
		public ushort operating_point_idc;
		public byte seq_level_idx;
		public byte seq_tier;
		public uint decoder_buffer_delay;
		public uint encoder_buffer_delay;
		public byte initial_display_delay_minus_1;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeAV1PictureInfoFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeAV1PictureInfo
	{
		public StdVideoEncodeAV1PictureInfoFlags flags;
		public StdVideoAV1FrameType frame_type;
		public uint frame_presentation_time;
		public uint current_frame_id;
		public byte order_hint;
		public byte primary_ref_frame;
		public byte refresh_frame_flags;
		public byte coded_denom;
		public ushort render_width_minus_1;
		public ushort render_height_minus_1;
		public StdVideoAV1InterpolationFilter interpolation_filter;
		public StdVideoAV1TxMode TxMode;
		public byte delta_q_res;
		public byte delta_lf_res;
		public fixed byte ref_order_hint[8];
		public fixed sbyte ref_frame_idx[7];
		public fixed byte reserved1[3];
		public fixed uint delta_frame_id_minus_1[7];
		public StdVideoAV1TileInfo* pTileInfo;
		public StdVideoAV1Quantization* pQuantization;
		public StdVideoAV1Segmentation* pSegmentation;
		public StdVideoAV1LoopFilter* pLoopFilter;
		public StdVideoAV1CDEF* pCDEF;
		public StdVideoAV1LoopRestoration* pLoopRestoration;
		public StdVideoAV1GlobalMotion* pGlobalMotion;
		public StdVideoEncodeAV1ExtensionHeader* pExtensionHeader;
		public uint* pBufferRemovalTimes;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeAV1ReferenceInfoFlags
	{
		public uint Value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public unsafe partial struct StdVideoEncodeAV1ReferenceInfo
	{
		public StdVideoEncodeAV1ReferenceInfoFlags flags;
		public uint RefFrameId;
		public StdVideoAV1FrameType frame_type;
		public byte OrderHint;
		public fixed byte reserved1[3];
		public StdVideoEncodeAV1ExtensionHeader* pExtensionHeader;
	}

}
